using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ui.Wpf.KanbanControl
{
    /// <summary>
    /// Логика взаимодействия для Kanban.xaml
    /// </summary>
    public partial class Kanban : UserControl, IKanbanBoardView
    {
        public Kanban()
        {
            showKanbanStrategy = new DefaultShowKanbanStrategey(this);

            elementsDispenser = new DefaultElementsDispenser();

            InitializeComponent();

            ShowKanbanBoard();
        }

        private void ShowKanbanBoard()
        {
            // ugly destory and build
            // TODO beautiful transition
            showKanbanStrategy.Show();
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
                ShowKanbanBoard();
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
                ShowKanbanBoard();
            }
        }

        public double SpliterWidth { get; private set; } = 1;

        public Brush SpliterBackground { get; private set; } = Brushes.Silver;

        Grid IKanbanBoardView.KanbanGrid => Grid;

        #endregion

        #region [ dependency property ]


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
            control.ShowKanbanBoard();
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
            control.ShowKanbanBoard();
        }

    #endregion
}
}
