using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ui.Wpf.KanbanControl.Behaviours;
using Ui.Wpf.KanbanControl.Elements;

namespace Ui.Wpf.KanbanControl
{
    /// <summary>
    /// Логика взаимодействия для Kanban.xaml
    /// </summary>
    public partial class Kanban : UserControl, IKanbanBoard
    {
        public Kanban()
        {
            showKanbanStrategy = new DefaultShowKanbanStrategey(this);

            InitializeComponent();
        }

        private void AddActionsToShow(KanbanChangeObjectType changeObjectType)
        {
            // ugly destory and build
            // TODO beautiful transition
            // TODO store changesets and animate last little part of it with some low frequency
            showKanbanStrategy.AddActionsToShow(changeObjectType);
        }

        private void SetCardItems(object oldValue, object newValue)
        {
            var oldNotifyableCollection = (INotifyCollectionChanged)oldValue;
            if (oldNotifyableCollection != null)
                oldNotifyableCollection.CollectionChanged -= OnItemsChanged;

            var newNotifyableCollection = (INotifyCollectionChanged)newValue;
            if (newNotifyableCollection != null)
                newNotifyableCollection.CollectionChanged += OnItemsChanged;

            BuildCards(this);
            AddActionsToShow(KanbanChangeObjectType.CardItems);
        }

        private void OnItemsChanged(
            object sender, 
            NotifyCollectionChangedEventArgs e)
        {
            BuildCards(this);
            AddActionsToShow(KanbanChangeObjectType.CardItems);
        }

        private void BuildCards(IKanbanBoard board)
        {
            board.Cards.Clear();
            foreach (var item in CardItems)
            {
                board.Cards.Add(new Card(item));
            }
        }

        #region [ IKanbanBoard ]

        List<Card> IKanbanBoard.Cards { get; } = new List<Card>();

        Grid IKanbanBoard.KanbanGrid => Grid;

        #endregion

        #region [ properties ]


        //TODO dependency properties

        private IShowKanbanStrategy showKanbanStrategy;
        public IShowKanbanStrategy ShowKanbanStrategy
        {
            get
            {
                return showKanbanStrategy;
            }
            set
            {
                showKanbanStrategy = value;
                AddActionsToShow(KanbanChangeObjectType.ShowStrategy);
            }
        }

        #endregion

        #region [ dependency property ]

        public System.Collections.IEnumerable CardItems
        {
            get { return (System.Collections.IEnumerable)GetValue(CardItemsProperty); }
            set { SetValue(CardItemsProperty, value); }
        }

        public static readonly DependencyProperty CardItemsProperty =
            DependencyProperty.Register("CardItems", 
                typeof(System.Collections.IEnumerable), 
                typeof(Kanban), 
                new PropertyMetadata(
                    new ObservableCollection<object>(),
                    OnCardItemsChanged));

        private static void OnCardItemsChanged(
            DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        {
            var control = (Kanban)obj;
            control.SetCardItems(e.OldValue, e.NewValue);
        }

        public ObservableCollection<IDimensionCategory> VerticalCategories
        {
            get { return (ObservableCollection<IDimensionCategory>)GetValue(VerticalCategoriesProperty); }
            set { SetValue(VerticalCategoriesProperty, value); }
        }

        public static readonly DependencyProperty VerticalCategoriesProperty =
            DependencyProperty.Register("VerticalCategories",
                typeof(ObservableCollection<IDimensionCategory>),
                typeof(Kanban), 
                new PropertyMetadata(
                    new ObservableCollection<IDimensionCategory>(),
                    OnVerticalCategoriesChanged));

        private static void OnVerticalCategoriesChanged(
            DependencyObject obj, 
            DependencyPropertyChangedEventArgs e)
        {
            var control = (Kanban)obj;
            control.AddActionsToShow(KanbanChangeObjectType.VerticalCategories);
        }

        public ObservableCollection<IDimensionCategory> HorisontalCategories
        {
            get { return (ObservableCollection<IDimensionCategory>)GetValue(HorisontalCategoriesProperty); }
            set { SetValue(HorisontalCategoriesProperty, value); }
        }

        public static readonly DependencyProperty HorisontalCategoriesProperty =
            DependencyProperty.Register("HorisontalCategories",
                typeof(ObservableCollection<IDimensionCategory>),
                typeof(Kanban), 
                new PropertyMetadata(
                    new ObservableCollection<IDimensionCategory>(),
                    OnHorizontalCategoriesChanged)
                );

        private static void OnHorizontalCategoriesChanged(
            DependencyObject obj,
            DependencyPropertyChangedEventArgs e)
        {
            var control = (Kanban)obj;
            control.AddActionsToShow(KanbanChangeObjectType.HorizontalCategories);
        }

        public double SpliterWidth
        {
            get { return (double)GetValue(SpliterWidthProperty); }
            set { SetValue(SpliterWidthProperty, value); }
        }

        public static readonly DependencyProperty SpliterWidthProperty =
            DependencyProperty.Register("SpliterWidth", typeof(double), typeof(Kanban), new PropertyMetadata(1d));

        public Brush SpliterBackground
        {
            get { return (Brush)GetValue(SpliterBackgroundProperty); }
            set { SetValue(SpliterBackgroundProperty, value); }
        }

        public static readonly DependencyProperty SpliterBackgroundProperty =
            DependencyProperty.Register("SpliterBackground", typeof(Brush), typeof(Kanban), new PropertyMetadata(Brushes.Silver));

        #endregion
    }
}
