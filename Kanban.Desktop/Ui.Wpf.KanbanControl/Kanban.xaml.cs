using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ui.Wpf.KanbanControl.Behaviours;
using Ui.Wpf.KanbanControl.Common;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements;
using Ui.Wpf.KanbanControl.Elements.CardElement;
using Ui.Wpf.KanbanControl.Expressions;

namespace Ui.Wpf.KanbanControl
{
    /// <summary>
    /// codebehind for Kanban.xaml
    /// </summary>
    public partial class Kanban : IKanbanBoard
    {
        public Kanban()
        {
            showKanbanStrategy = new DefaultShowKanbanStrategey(this);
            InitializeComponent();
            BuildCommands();
        }

        private void AddActionsToShow(KanbanChangeObjectType changeObjectType)
        {
            // ugly destory and build
            // TODO beautiful transition
            // TODO store changesets and animate last little part of it with some low frequency

            if (HorizontalDimension == null
                || VerticalDimension == null
                || Cards == null)
                return;

            // TODO create only when type changed
            propertyAccessors = new PropertyAccessorsExpressionCreator(Cards);
            BuildAutoCategories(Cards, HorizontalDimension, VerticalDimension, propertyAccessors);

            if (HorizontalDimension.Categories == null
                || HorizontalDimension.Categories.Count == 0
                || VerticalDimension.Categories == null
                || VerticalDimension.Categories.Count == 0)
                return;


            BuildCards();
            BuildCells();
            BuildHeaders();            

            showKanbanStrategy.AddActionsToShow(
                changeObjectType, 
                propertyAccessors);
        }

        private void BuildAutoCategories(
            IEnumerable Cards, 
            IDimension horizontalDimension, 
            IDimension verticalDimension, 
            PropertyAccessorsExpressionCreator propertyAccessors)
        {
            if (horizontalDimension is IDynamicDimension dynamicHorizontalDimension)
            {
                horizontalDimension.Categories = GetDimensionCategories(Cards, dynamicHorizontalDimension, propertyAccessors);
            }

            if (verticalDimension is IDynamicDimension dynamicVerticalDimension)
            {
                verticalDimension.Categories = GetDimensionCategories(Cards, dynamicVerticalDimension, propertyAccessors);
            }
        }

        private static IList<IDimensionCategory> GetDimensionCategories(
            IEnumerable Cards,
            IDynamicDimension dimension, 
            PropertyAccessorsExpressionCreator propertyAccessors)
        {
            var getElementCategory = propertyAccessors.TakeGetterForProperty(dimension.FieldName);

            HashSet<object> tagFilter = null;
            if (dimension.Tags != null)
            {
                tagFilter = new HashSet<object>(dimension.Tags);
            }

            if (getElementCategory != null)
            {
                var categories = Cards.Cast<object>()
                    .Select(i => getElementCategory(i))
                    .Where(i => i != null)
                    .Where(i=> tagFilter == null || tagFilter.Contains(i.ToString()))
                    .OrderBy(i => i)
                    .Distinct()
                    .Select(i => (IDimensionCategory)new TagsDimensionCategory(i.ToString(), i))
                    .ToList();

                return categories;
            }

            return null;
        }

        private void BuildCells()
        {
            cells = new Cell[HorizontalDimension.Categories.Count, VerticalDimension.Categories.Count];
            for (int i = 0; i < HorizontalDimension.Categories.Count; i++)
            {
                for (int j = 0; j < VerticalDimension.Categories.Count; j++)
                {
                    cells[i, j] = new Cell(new CellView());
                }
            }
        }

        private void SetCards(object oldValue, object newValue)
        {
            var oldNotifyableCollection = (INotifyCollectionChanged)oldValue;
            if (oldNotifyableCollection != null)
                oldNotifyableCollection.CollectionChanged -= OnItemsChanged;

            var newNotifyableCollection = (INotifyCollectionChanged)newValue;
            if (newNotifyableCollection != null)
                newNotifyableCollection.CollectionChanged += OnItemsChanged;

            AddActionsToShow(KanbanChangeObjectType.Cards);
        }

        private void OnItemsChanged(
            object sender, 
            NotifyCollectionChangedEventArgs e)
        {
            AddActionsToShow(KanbanChangeObjectType.Cards);
        }

        private void BuildCards()
        {
            var contentItems = CardContent?.CardContentItems
                .Select(ci => new
                {
                    ci,
                    getter = propertyAccessors.TakeGetterForProperty(ci.Path)
                })
                .Where(x => x.getter != null)
                .ToArray();

            cardElements.Clear();
            foreach (var cardData in Cards)
            {
                var cardElement = new Card(cardData);

                if (contentItems != null)
                {
                    cardElement.ContentItems = contentItems
                        .Where(x => x.ci.Area == CardContentArea.Main)
                        .Select(g => new ContentItem(cardData, g.getter))
                        .ToList();

                    cardElement.ShortContentItems = contentItems
                        .Where(x => x.ci.Area == CardContentArea.Short)
                        .Select(g => new ContentItem(cardData, g.getter))
                        .ToList();

                    cardElement.AdditionalContentItems = contentItems
                        .Where(x => x.ci.Area == CardContentArea.Additional)
                        .Select(g => new ContentItem(cardData, g.getter))
                        .ToList();
                }

                if (CardsColors != null)
                {
                    var colorKeyGetter = propertyAccessors.TakeGetterForProperty(CardsColors.Path);

                    ICardColor colors;
                    if (CardsColors.ColorMap.TryGetValue(colorKeyGetter(cardData).ToString(), out colors))
                    {
                        cardElement.Background = colors.Background;
                        cardElement.BorderBrush = colors.BorderBrush;
                    }

                }

                cardElement.ActionItems = cardElement.AdditionalContentItems
                        .Select(g => new ActionItem(g))
                        .ToList();

                cardElement.View.ContentTemplate = CardTemplate;

                cardElements.Add(cardElement);
            }
        }

        private void BuildHeaders()
        {
            horizontalHeaders.Clear();
            for (int i = 0; i < HorizontalDimension.Categories.Count; i++)
            {
                var head = new HeaderView();
                head.ContentTemplate = HorizontalHeaderTemplate;
                head.Content = HorizontalDimension.Categories[i];
                horizontalHeaders.Add(new Header(head));
            }

            verticalHeaders.Clear();
            for (int j = 0; j < VerticalDimension.Categories.Count; j++)
            {
                var head = new HeaderView();
                head.ContentTemplate = VerticalHeaderTemplate;
                head.Content = VerticalDimension.Categories[j];
                verticalHeaders.Add(new Header(head));
            }
        }

        private void BuildCommands()
        {
            PrintCommand = new DelegateCommand((o) =>
            {
                var printDialog = new PrintDialog();
                var result = printDialog.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    printDialog.PrintVisual(this, "");
                }
            });
        }

        #region [ IKanbanBoard ]

        private readonly List<Header> verticalHeaders = new List<Header>();
        List<Header> IKanbanBoard.VerticalHeaders => verticalHeaders;

        private readonly List<Header> horizontalHeaders = new List<Header>();
        List<Header> IKanbanBoard.HorizontalHeaders => horizontalHeaders;

        private readonly List<Card> cardElements = new List<Card>();
        List<Card> IKanbanBoard.CardElements => cardElements;

        private Cell[,] cells;
        Cell[,] IKanbanBoard.Cells => cells;

        Grid IKanbanBoard.KanbanGrid => Grid;

        #endregion

        #region [ properties ]

        private static readonly DefaultTemplates defaultTemplates = new DefaultTemplates();

        //TODO dependency properties
        private IShowKanbanStrategy showKanbanStrategy;
        public IShowKanbanStrategy ShowKanbanStrategy
        {
            get => showKanbanStrategy;
            set
            {
                showKanbanStrategy = value;
                AddActionsToShow(KanbanChangeObjectType.ShowStrategy);
            }
        }

        private PropertyAccessorsExpressionCreator propertyAccessors;

        #endregion

        #region [ dependency properties ]

        public IEnumerable Cards
        {
            get => (IEnumerable)GetValue(CardsProperty);
            set => SetValue(CardsProperty, value);
        }

        public static readonly DependencyProperty CardsProperty =
            DependencyProperty.Register("Cards", 
                typeof(IEnumerable), 
                typeof(Kanban), 
                new PropertyMetadata(
                    new ObservableCollection<object>(),
                    OnCardsChanged));

        private static void OnCardsChanged(
            DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        {
            var control = (Kanban)obj;
            control.SetCards(e.OldValue, e.NewValue);
        }

        public ICardContent CardContent
        {
            get { return (ICardContent)GetValue(CardContentProperty); }
            set { SetValue(CardContentProperty, value); }
        }

        public static readonly DependencyProperty CardContentProperty =
            DependencyProperty.Register(
                "CardContent", 
                typeof(ICardContent), 
                typeof(Kanban), 
                new PropertyMetadata(
                    new CardContent(new CardContentItem[] 
                    {
                        new CardContentItem("Subject"),
                        new CardContentItem("Tracker")
                    }),
                    OnCardContentChanged));

        private static void OnCardContentChanged(
            DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        {
            var control = (Kanban)obj;
            control.AddActionsToShow(KanbanChangeObjectType.CardContentItems);
        }

        public IDimension VerticalDimension
        {
            get => (IDimension)GetValue(VerticalDimensionProperty);
            set => SetValue(VerticalDimensionProperty, value);
        }

        public static readonly DependencyProperty VerticalDimensionProperty =
            DependencyProperty.Register("VerticalDimension", 
                typeof(IDimension), 
                typeof(Kanban), 
                new PropertyMetadata(
                    null,
                    OnVerticalDimensionChanged));

        private static void OnVerticalDimensionChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (Kanban)obj;
            control.AddActionsToShow(KanbanChangeObjectType.VerticalCategories);
        }

        public IDimension HorizontalDimension
        {
            get => (IDimension)GetValue(HorizontalDimensionProperty);
            set => SetValue(HorizontalDimensionProperty, value);
        }

        public static readonly DependencyProperty HorizontalDimensionProperty =
            DependencyProperty.Register("HorizontalDimension",
                typeof(IDimension),
                typeof(Kanban),
                new PropertyMetadata(
                    null,
                    HorizontalDimensionChanged));

        private static void HorizontalDimensionChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (Kanban)obj;
            control.AddActionsToShow(KanbanChangeObjectType.HorizontalCategories);
        }

        public ICardsColors CardsColors
        {
            get { return (ICardsColors)GetValue(CardsColorsProperty); }
            set { SetValue(CardsColorsProperty, value); }
        }

        public static readonly DependencyProperty CardsColorsProperty =
            DependencyProperty.Register("CardsColors",
                typeof(ICardsColors),
                typeof(Kanban),
                new PropertyMetadata(null, CardsColorsChanged));

        private static void CardsColorsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var control = (Kanban)obj;
            control.AddActionsToShow(KanbanChangeObjectType.CardsColors);
        }

        public double SpliterWidth
        {
            get => (double)GetValue(SpliterWidthProperty);
            set => SetValue(SpliterWidthProperty, value);
        }

        public static readonly DependencyProperty SpliterWidthProperty =
            DependencyProperty.Register("SpliterWidth", typeof(double), typeof(Kanban), new PropertyMetadata(4d));

        public Brush SpliterBackground
        {
            get => (Brush)GetValue(SpliterBackgroundProperty);
            set => SetValue(SpliterBackgroundProperty, value);
        }

        public static readonly DependencyProperty SpliterBackgroundProperty =
            DependencyProperty.Register("SpliterBackground", typeof(Brush), typeof(Kanban), new PropertyMetadata(Brushes.Transparent));


        public DataTemplate CardTemplate
        {
            get => (DataTemplate)GetValue(CardTemplateProperty);
            set => SetValue(CardTemplateProperty, value);
        }

        public static readonly DependencyProperty CardTemplateProperty =
            DependencyProperty.Register("CardTemplate", 
                typeof(DataTemplate), 
                typeof(Kanban), 
                new PropertyMetadata(
                    defaultTemplates["CardTemplate"]));


        public DataTemplate CellTemplate
        {
            get => (DataTemplate)GetValue(CellTemplateProperty);
            set => SetValue(CellTemplateProperty, value);
        }

        public static readonly DependencyProperty CellTemplateProperty =
            DependencyProperty.Register("CellTemplate",
                typeof(DataTemplate),
                typeof(Kanban),
                new PropertyMetadata());

        public DataTemplate HorizontalHeaderTemplate
        {
            get => (DataTemplate)GetValue(HorizontalHeaderTemplateProperty);
            set => SetValue(HorizontalHeaderTemplateProperty , value);
        }

        public static readonly DependencyProperty HorizontalHeaderTemplateProperty =
            DependencyProperty.Register("HorizontalHeaderTemplate", 
                typeof(DataTemplate), 
                typeof(Kanban), 
                new PropertyMetadata(
                    defaultTemplates["DefaultHorizontalHeaderTemplate"]));
        
        public DataTemplate VerticalHeaderTemplate
        {
            get => (DataTemplate)GetValue(VerticalHeaderTemplateProperty);
            set => SetValue(VerticalHeaderTemplateProperty , value);
        }

        public static readonly DependencyProperty VerticalHeaderTemplateProperty =
            DependencyProperty.Register("VerticalHeaderTemplate", 
                typeof(DataTemplate), 
                typeof(Kanban), 
                new PropertyMetadata(
                    defaultTemplates["DefaulVerticalHeaderTemplate"]));

        public ICommand PrintCommand
        {
            get { return (ICommand)GetValue(PrintCommandProperty); }
            private set { SetValue(PrintCommandProperty, value); }
        }

        public static readonly DependencyProperty PrintCommandProperty =
            DependencyProperty.Register("PrintCommand", 
                typeof(ICommand), 
                typeof(Kanban), 
                new PropertyMetadata());

        #endregion
    }
}
