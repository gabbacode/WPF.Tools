using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Ui.Wpf.KanbanControl.Behaviours;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements;

namespace Ui.Wpf.KanbanControl
{
    /// <summary>
    /// codebehind for Kanban.xaml
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
            if (HorizontalDimension == null
                || HorizontalDimension.Categories.Count == 0
                || VerticalDimension == null
                || VerticalDimension.Categories.Count == 0)
                return;


            BuildCells();
            BuildCards();

            showKanbanStrategy.AddActionsToShow(changeObjectType);
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

        private void SetCardItems(object oldValue, object newValue)
        {
            var oldNotifyableCollection = (INotifyCollectionChanged)oldValue;
            if (oldNotifyableCollection != null)
                oldNotifyableCollection.CollectionChanged -= OnItemsChanged;

            var newNotifyableCollection = (INotifyCollectionChanged)newValue;
            if (newNotifyableCollection != null)
                newNotifyableCollection.CollectionChanged += OnItemsChanged;

            AddActionsToShow(KanbanChangeObjectType.CardItems);
        }

        private void OnItemsChanged(
            object sender, 
            NotifyCollectionChangedEventArgs e)
        {
            AddActionsToShow(KanbanChangeObjectType.CardItems);
        }

        private void BuildCards()
        {
            cards.Clear();
            foreach (var item in CardItems)
            {
                cards.Add(new Card(item));
            }
        }

        #region [ IKanbanBoard ]

        private List<Card> cards = new List<Card>();
        List<Card> IKanbanBoard.Cards
        {
            get
            {
                return cards;
            }
        } 

        private Cell[,] cells;
        Cell[,] IKanbanBoard.Cells {
            get
            {
                return cells;
            }
        }

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

        public IDimension VerticalDimension
        {
            get { return (IDimension)GetValue(VerticalDimensionProperty); }
            set { SetValue(VerticalDimensionProperty, value); }
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
            get { return (IDimension)GetValue(HorizontalDimensionProperty); }
            set { SetValue(HorizontalDimensionProperty, value); }
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
