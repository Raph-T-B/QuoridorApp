using Microsoft.Maui.Layouts;
using Microsoft.Maui.Controls;

namespace QuoridorMaui.MyLayouts
{
    public class MatrixLayout : Layout
    {
        public static readonly BindableProperty NbColumnsProperty 
            = BindableProperty.Create(nameof(NbColumns), 
                                    typeof(int),
                                    typeof(MatrixLayout),
                                    10);

        public int NbColumns
        {
            get { return (int)GetValue(NbColumnsProperty); }
            set { SetValue(NbColumnsProperty, value); }
        }

        public static readonly BindableProperty NbRowsProperty 
            = BindableProperty.Create(nameof(NbRows), 
                                    typeof(int), 
                                    typeof(MatrixLayout), 
                                    10);

        public int NbRows
        {
            get { return (int)GetValue(NbRowsProperty); }
            set { SetValue(NbRowsProperty, value); }
        }

        public static readonly BindableProperty HorizontalSpacingProperty 
            = BindableProperty.Create(nameof(HorizontalSpacing), 
                                    typeof(double), 
                                    typeof(MatrixLayout), 
                                    0.0);

        public double HorizontalSpacing
        {
            get { return (double)GetValue(HorizontalSpacingProperty); }
            set { SetValue(HorizontalSpacingProperty, value); }
        }

        public static readonly BindableProperty VerticalSpacingProperty 
            = BindableProperty.Create(nameof(VerticalSpacing), 
                                    typeof(double), 
                                    typeof(MatrixLayout), 
                                    0.0);

        public double VerticalSpacing
        {
            get { return (double)GetValue(VerticalSpacingProperty); }
            set { SetValue(VerticalSpacingProperty, value); }
        }

        public View GetCellAt(int x, int y)
        {
            if (x < 0 || x >= NbColumns || y < 0 || y >= NbRows)
                return null;

            int index = y * NbColumns + x;
            if (index < 0 || index >= Children.Count)
                return null;

            return (View)Children[index];
        }

        public (int x, int y)? GetCellPosition(View cell)
        {
            int index = Children.IndexOf(cell);
            if (index == -1)
                return null;

            int x = index % NbColumns;
            int y = index / NbColumns;
            return (x, y);
        }

        protected override ILayoutManager CreateLayoutManager()
        {
            return new MatrixLayoutManager(this);
        }
    }
} 