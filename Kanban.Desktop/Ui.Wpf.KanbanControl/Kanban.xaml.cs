using System;
using System.Collections.ObjectModel;
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

            elementsDispenser = new DefaultElementsDispenser();

            InitializeComponent();

            ShowKanbanBoard(
                KanbanChangeObjectType.HorizontalCategories
                | KanbanChangeObjectType.VerticalCategories
                | KanbanChangeObjectType.Items);
        }

        private void ShowKanbanBoard(KanbanChangeObjectType changeObjectType)
        {
            // ugly destory and build
            // TODO beautiful transition
            // TODO store changesets and animate last little part of it with some low frequency
            showKanbanStrategy.Show(changeObjectType);
        }


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
                ShowKanbanBoard(KanbanChangeObjectType.ShowStrategy);
            }
        }

        private IElementsDispenser elementsDispenser;
        public IElementsDispenser ElementsDispenser
        {
            get
            {
                return elementsDispenser;
            }
            set
            {
                elementsDispenser = value;
                ShowKanbanBoard(KanbanChangeObjectType.DispenseStrategy);
            }
        }

        public double SpliterWidth { get; private set; } = 1;

        public Brush SpliterBackground { get; private set; } = Brushes.Silver;

        Grid IKanbanBoard.KanbanGrid => Grid;

        #endregion

        #region [ dependency property ]

        public ObservableCollection<object> Items
        {
            get { return (ObservableCollection<object>)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ObservableCollection<object>), typeof(Kanban), new PropertyMetadata(0));


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
            control.ShowKanbanBoard(KanbanChangeObjectType.VerticalCategories);
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
            control.ShowKanbanBoard(KanbanChangeObjectType.HorizontalCategories);
        }

    #endregion
}
}
