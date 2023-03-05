namespace Liquid
{
    public class LiquidCellularAutomation
    {
        public const sbyte BLOCKED = -1;
        public const sbyte EMPTY = 0;
        public const sbyte FILLED = 1;

        public sbyte[,] cells;
        private readonly int width;
        private readonly int height;

        public LiquidCellularAutomation(int width, int height)
        {
            this.width = width;
            this.height = height;
            cells = new sbyte[height, width];
        }

        public void Automate()
        {
            for (int y = 0; y < height; y++)
            {
                bool  isOdd= y % 2 != 0;
                for (int x = isOdd ? width - 1 : 0; x >= 0 && x < width; x += isOdd ? -1 : 1)
                {
                    if (cells[y, x] != FILLED)
                    {
                        continue;
                    }

                    cells[y, x] = EMPTY;
                    if (isOdd)
                    {
                        if (FillIfEmpty(x, y - 1)) {}
                        else if (FillIfEmpty(x + 1, y - 1)) {}
                        else if (FillIfEmpty(x - 1, y - 1)) {}
                        else if (FillIfEmpty(x - 1, y)) {}
                        else if (FillIfEmpty(x + 1, y)) {}
                        else {cells[y, x] = 1;}
                    }
                    else
                    {
                        if (FillIfEmpty(x, y - 1)) {}
                        else if (FillIfEmpty(x - 1, y - 1)) {}
                        else if (FillIfEmpty(x + 1, y - 1)) {}
                        else if (FillIfEmpty(x + 1, y)) {}
                        else if (FillIfEmpty(x - 1, y)) {}
                        else {cells[y, x] = 1;}
                    }
                }
            }
        }

        private bool FillIfEmpty(int x, int y)
        {
            bool isEmpty = x >= 0 && x < width && y >= 0 && y < height && cells[y, x] == 0;
            if (isEmpty)
            {
                cells[y, x] = FILLED;
            }
            return isEmpty;
        }

        private void SetCells(sbyte cell, int xMin, int yMin, int width, int height)
        {
            for (int y = yMin; y < yMin + height; y++)
            {
                for (int x = xMin; x < xMin + width; x++)
                {
                    if (cells[y, x] != BLOCKED)
                    {
                        cells[y, x] = cell;
                    }
                }
            }
        }

        public void BlockCells(int x, int y, int width, int height)
        {
            SetCells(BLOCKED, x, y, width, height);
        }

        public void FillCells(int x, int y, int width, int height)
        {
            SetCells(FILLED, x, y, width, height);
        }

        public void EmptyCells()
        {
            SetCells(EMPTY, 0, 0, width, height);
        }
    }
}
