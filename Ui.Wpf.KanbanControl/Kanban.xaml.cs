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
using Ui.Wpf.KanbanControl.Common;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements;
using Ui.Wpf.KanbanControl.ElementsManagement;
using Ui.Wpf.KanbanControl.Elements.CardElement;
using Ui.Wpf.KanbanControl.Expressions;
using System.Diagnostics;

namespace Ui.Wpf.KanbanControl
{
    /// <summary>
    /// codebehind for Kanban.xaml
    /// </summary>
    public partial class Kanban : IKanbanBoard
    {
        public Kanban()
        {
            kanbanManager = new KanbanManager(this);
            categoriesManager = new CategoriesManager();

            InitializeComponent();
            BuildCommands();
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            // let the GridSplitter move
            if (sizeInfo.NewSize.Height != 0
                && double.IsNaN(Height))
            {
                Height = sizeInfo.NewSize.Height;
            }
        }

        private void AddActionsToShow(KanbanChangeObjectType changeObjectType)
        {
            Height = double.NaN;
            // ugly destory and build
            // TODO beautiful transition
            // TODO store changesets and animate last little part of it with some low frequency


            if (HorizontalDimension  == null
                || VerticalDimension == null)
            {
                ClearHeaders();
            }

            ClearCards();

            if (cells != null)
                foreach (var cell in cells)
                {
                    cell.View.MouseDoubleClick -= CardElementMouseDoubleClick;
                } //todo:change clearcard method and add clearcell

            // TODO create only when type changed
            propertyAccessors = new PropertyAccessorsExpressionCreator(Cards);

            categoriesManager.BuildAutoCategories(
                Cards,
                HorizontalDimension, VerticalDimension,
                propertyAccessors);

            BuildCards();
            BuildCells();
            BuildHeaders();


            kanbanManager.AddActionsToShow(
                changeObjectType,
                propertyAccessors);
        }

        private void BuildCells()
        {
            Debug.WriteLine($"BuildCells old cells: {cells?.GetHashCode()}");

            if (HorizontalDimension?.Categories == null
                || HorizontalDimension?.Categories.Count == 0
                || VerticalDimension?.Categories == null
                || VerticalDimension?.Categories.Count == 0)
            {
                cells = new Cell[0, 0];
                Debug.WriteLine($"BuildCells new cells: {cells?.GetHashCode()}");
                return;
            }

            cells = new Cell[HorizontalDimension.Categories.Count, VerticalDimension.Categories.Count];
            for (int i = 0; i < HorizontalDimension.Categories.Count; i++)
            {
                for (int j = 0; j < VerticalDimension.Categories.Count; j++)
                {
                    cells[i, j] = new Cell(new CellView());
                    cells[i, j].View.MouseDoubleClick += CardElementMouseDoubleClick;
                }
            }
            Debug.WriteLine($"BuildCells new cells: {cells?.GetHashCode()}");
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
                    getter = propertyAccessors.TakeGetterForProperty(ci.ExpressionPath)
                })
                .Where(x => x.getter != null)
                .ToArray();

            var brushConverter = new BrushConverter();

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

                    if (CardsColors.ColorMap.TryGetValue(colorKeyGetter(cardData), out var colors))
                    {
                        cardElement.Background = (SolidColorBrush)(brushConverter).ConvertFromString(colors.Background);
                        cardElement.BorderBrush = (SolidColorBrush)(brushConverter).ConvertFromString(colors.BorderBrush);
                    }

                }

                cardElement.ActionItems = cardElement.AdditionalContentItems
                        .Select(g => new ActionItem(g))
                        .ToList();

                cardElement.View.ContentTemplate = CardTemplate;
                cardElement.View.MouseLeftButtonDown += CardMouseClick;
                cardElements.Add(cardElement);
            }
        }

        private void ClearCards()
        {
            foreach (var card in cardElements)
            {
                card.View.MouseLeftButtonDown -= CardMouseClick;
            }
            cardElements.Clear();
        }

        private void CardElementMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as FrameworkElement;
            var dc = source?.DataContext;

            if (dc is ContentItem ci)
            {
                CardMouseDoubleClickCommand?.Execute(ci.DataItem);
            }
            else if (dc is Card c)
            {
                CardMouseDoubleClickCommand?.Execute(c.Item);
            }

            else CardMouseDoubleClickCommand?.Execute(null);
        }

        private void CardMouseClick(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as FrameworkElement;
            var dc     = source?.DataContext;

            if (dc is ContentItem ci)
            {
                CardMouseClickCommand?.Execute(ci.DataItem);
            }
            else if (dc is Card c)
            {
                CardMouseClickCommand?.Execute(c.Item);
            }
        }

        private void HorizontalHeaderMouseClick(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as FrameworkElement;
            var dc     = source?.DataContext;
            if (dc is IDimensionCategory h)
            {
                HorizontalHeaderMouseClickCommand?.Execute(h.Caption);
            }
            else
            {
                CardMouseClickCommand?.Execute(null);
            }
        }

        private void HorizontalHeaderDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as FrameworkElement;
            var dc = source?.DataContext;
            if (dc is IDimensionCategory h)
            {
                HorizontalHeaderDoubleClickCommand?.Execute(h.Caption);
            }
            else
            {
                CardMouseClickCommand?.Execute(null);
            }
        }

        private void VerticalHeaderMouseClick(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as FrameworkElement;
            var dc     = source?.DataContext;
            if (dc is IDimensionCategory h)
            {
                VerticalHeaderMouseClickCommand?.Execute(h.Caption);
            }
            else
            {
                CardMouseClickCommand?.Execute(null);
            }
        }

        private void VerticalHeaderDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var source = e.OriginalSource as FrameworkElement;
            var dc = source?.DataContext;
            if (dc is IDimensionCategory h)
            {
                VerticalHeaderDoubleClickCommand?.Execute(h.Caption);
            }
            else
            {
                CardMouseClickCommand?.Execute(null);
            }
        }

        private void ClearHeaders()
        {
            foreach (var header in horizontalHeaders)
            {
                header.View.MouseLeftButtonDown -= HorizontalHeaderMouseClick;
                header.View.MouseDoubleClick -= HorizontalHeaderDoubleClick;
            }

            foreach (var header in verticalHeaders)
            {
                header.View.MouseLeftButtonDown -= VerticalHeaderMouseClick;
                header.View.MouseDoubleClick -= VerticalHeaderDoubleClick;
            }

            horizontalHeaders.Clear();
            verticalHeaders.Clear();
        }

        private void BuildHeaders()
        {
            for (int i = 0; i < HorizontalDimension?.Categories?.Count; i++)
            {
                var head = new HeaderView();
                head.ContentTemplate = HorizontalHeaderTemplate;
                head.Content = HorizontalDimension.Categories[i];
                horizontalHeaders.Add(new Header(head));
                head.MouseLeftButtonDown += HorizontalHeaderMouseClick;
                head.MouseDoubleClick += HorizontalHeaderDoubleClick;
            }

            for (int j = 0; j < VerticalDimension?.Categories?.Count; j++)
            {
                var head = new HeaderView();
                head.ContentTemplate = VerticalHeaderTemplate;
                head.Content = VerticalDimension.Categories[j];
                verticalHeaders.Add(new Header(head));
                head.MouseLeftButtonDown += VerticalHeaderMouseClick;
                head.MouseDoubleClick += VerticalHeaderDoubleClick;
            }
        }

        private void BuildCommands()
        {
            PrintCommand = new DelegateCommand((o) =>
            {
                var printDialog = new PrintDialog();
                var result = printDialog.ShowDialog();
                if (result.Value)
                {
                    var capabilities = printDialog.PrintQueue.GetPrintCapabilities(printDialog.PrintTicket);

                    var scale = Math.Min(
                        capabilities.PageImageableArea.ExtentWidth / ActualWidth, 
                        capabilities.PageImageableArea.ExtentHeight / ActualHeight);

                    LayoutTransform = new ScaleTransform(scale, scale);

                    var pageSize = new Size(
                        capabilities.PageImageableArea.ExtentWidth, 
                        capabilities.PageImageableArea.ExtentHeight);

                    Measure(pageSize);
                    Arrange(new Rect(
                        new Point(
                            capabilities.PageImageableArea.OriginWidth, 
                            capabilities.PageImageableArea.OriginHeight), 
                        pageSize));

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

        private PropertyAccessorsExpressionCreator propertyAccessors;

        private readonly IKanbanManager kanbanManager;

        private readonly CategoriesManager categoriesManager;

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
                    new CardContent(new ICardContentItem[] 
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
            get => (ICardsColors)GetValue(CardsColorsProperty);
            set => SetValue(CardsColorsProperty, value);
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
            get => (ICommand)GetValue(PrintCommandProperty);
            private set => SetValue(PrintCommandProperty, value);
        }

        public static readonly DependencyProperty PrintCommandProperty =
            DependencyProperty.Register("PrintCommand", 
                typeof(ICommand), 
                typeof(Kanban), 
                new PropertyMetadata());

        public ICommand CardMouseDoubleClickCommand
        {
            get => (ICommand)GetValue(CardMouseDoubleClickCommandProperty);
            set => SetValue(CardMouseDoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty CardMouseDoubleClickCommandProperty =
            DependencyProperty.Register(
                "CardMouseDoubleClickCommand", 
                typeof(ICommand), typeof(Kanban), 
                new PropertyMetadata(null));

        public ICommand CardMouseClickCommand
        {
            get => (ICommand)GetValue(CardMouseClickCommandProperty);
            set => SetValue(CardMouseClickCommandProperty, value);
        }

        public static readonly DependencyProperty CardMouseClickCommandProperty =
            DependencyProperty.Register(
                "CardMouseClickCommand",
                typeof(ICommand), typeof(Kanban),
                new PropertyMetadata(null));

        public ICommand HorizontalHeaderMouseClickCommand
        {
            get => (ICommand)GetValue(HorizontalHeaderMouseClickCommandProperty);
            set => SetValue(HorizontalHeaderMouseClickCommandProperty, value);
        }

        public static readonly DependencyProperty HorizontalHeaderMouseClickCommandProperty =
            DependencyProperty.Register(
                "HorizontalHeaderMouseClickCommand",
                typeof(ICommand), typeof(Kanban),
                new PropertyMetadata(null));

        public ICommand HorizontalHeaderDoubleClickCommand
        {
            get => (ICommand)GetValue(HorizontalHeaderDoubleClickCommandProperty);
            set => SetValue(HorizontalHeaderDoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty HorizontalHeaderDoubleClickCommandProperty =
            DependencyProperty.Register(
                "HorizontalHeaderDoubleClickCommand",
                typeof(ICommand), typeof(Kanban),
                new PropertyMetadata(null));

        public ICommand VerticalHeaderMouseClickCommand
        {
            get => (ICommand)GetValue(VerticalHeaderMouseClickCommandProperty);
            set => SetValue(VerticalHeaderMouseClickCommandProperty, value);
        }

        public static readonly DependencyProperty VerticalHeaderMouseClickCommandProperty =
            DependencyProperty.Register(
                "VerticalHeaderMouseClickCommand",
                typeof(ICommand), typeof(Kanban),
                new PropertyMetadata(null));

        public ICommand VerticalHeaderDoubleClickCommand
        {
            get => (ICommand)GetValue(VerticalHeaderDoubleClickCommandProperty);
            set => SetValue(VerticalHeaderDoubleClickCommandProperty, value);
        }

        public static readonly DependencyProperty VerticalHeaderDoubleClickCommandProperty =
            DependencyProperty.Register(
                "VerticalHeaderDoubleClickCommand",
                typeof(ICommand), typeof(Kanban),
                new PropertyMetadata(null));
        #endregion
    }
}
