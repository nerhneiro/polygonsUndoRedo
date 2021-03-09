using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
namespace test
{
    public partial class Form1 : Form
    {
        ToolStripMenuItem squareChoice = new ToolStripMenuItem("Square") { Checked = false, CheckOnClick = true };
        ToolStripMenuItem triangleChoice = new ToolStripMenuItem("Triangle") { Checked = false, CheckOnClick = true };
        ToolStripMenuItem circleChoice = new ToolStripMenuItem("Circle") { Checked = true, CheckOnClick = true };
        ToolStripMenuItem fileMenu = new ToolStripMenuItem("File");
        ToolStripMenuItem shapeMenu = new ToolStripMenuItem("Figure");
        ToolStripMenuItem algorithmMenu = new ToolStripMenuItem("Algorithm");
        ToolStripMenuItem jarvisChoice = new ToolStripMenuItem("Jarvis") { Checked = true, CheckOnClick = true };
        ToolStripMenuItem simpleChoice = new ToolStripMenuItem("Definition") { Checked = true, CheckOnClick = true };
        ToolStripMenuItem settingsMenu = new ToolStripMenuItem("Settings");
        ToolStripMenuItem radiusChoice = new ToolStripMenuItem("Radius");
        ToolStripMenuItem colorsFillInChoice = new ToolStripMenuItem("Choose shape color");
        ToolStripMenuItem colorsLineChoice = new ToolStripMenuItem("Choose line color");
        ToolStripMenuItem playChoice = new ToolStripMenuItem("Play");
        ToolStripMenuItem stopChoice = new ToolStripMenuItem("Stop");
        ToolStripMenuItem saveChoice = new ToolStripMenuItem("Save");
        ToolStripMenuItem saveasChoice = new ToolStripMenuItem("Save as");
        ToolStripMenuItem openChoice = new ToolStripMenuItem("Open");
        ToolStripMenuItem newChoice = new ToolStripMenuItem("New");
        Form2 f2 = null;
        Random random;
        bool dinamics = false;
        string filename = "";
        bool filechanges = false;
        public Form1()
        {
            InitializeComponent();
            algorithmMenu.DropDownItems.Add(simpleChoice);
            algorithmMenu.DropDownItems.Add(jarvisChoice);
            shapeMenu.DropDownItems.Add(circleChoice);
            shapeMenu.DropDownItems.Add(squareChoice);
            shapeMenu.DropDownItems.Add(triangleChoice);
            circleChoice.CheckedChanged += circle_CheckedChanged;
            triangleChoice.CheckedChanged += triangle_CheckedChanged;
            squareChoice.CheckedChanged += square_CheckedChanged;
            circleChoice.Enabled = true;
            jarvisChoice.Enabled = true;
            simpleChoice.CheckState = CheckState.Unchecked;
            jarvisChoice.CheckedChanged += jarvis_CheckedChanged;
            simpleChoice.CheckedChanged += simple_CheckedChanged;
            menuStrip1.Items.Add(fileMenu);
            menuStrip1.Items.Add(shapeMenu);
            menuStrip1.Items.Add(algorithmMenu);
            menuStrip1.Items.Add(settingsMenu);
            menuStrip1.Items.Add(playChoice);
            menuStrip1.Items.Add(stopChoice);

            settingsMenu.DropDownItems.Add(radiusChoice);
            settingsMenu.DropDownItems.Add(colorsFillInChoice);
            settingsMenu.DropDownItems.Add(colorsLineChoice);

            fileMenu.DropDownItems.Add(newChoice);
            fileMenu.DropDownItems.Add(openChoice);
            fileMenu.DropDownItems.Add(saveChoice);
            fileMenu.DropDownItems.Add(saveasChoice);

            saveasChoice.Click += saveasChoice_Click;
            saveChoice.Click += saveChoice_Click;
            openChoice.Click += openChoice_Click;
            newChoice.Click += newChoice_Click;
            radiusChoice.Enabled = true;
            radiusChoice.Click += radiusChoice_Click;
            colorsFillInChoice.Click += colors_fill_in_CheckedChanged;
            colorsLineChoice.Click += colors_line_CheckedChanged;
            stopChoice.Click += stopChoise_Click;
            playChoice.Click += playChoise_Click;

            random = new Random();

        }
        void playChoise_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }
        void stopChoise_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }
        void SaveState()
        {
            saveFileDialog1.Filter = "Binary File(*.bin) | *.bin";
            saveFileDialog1.ShowDialog();
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(saveFileDialog1.FileName, FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, points);
                bf.Serialize(fs, Shape.R);
                bf.Serialize(fs, Shape.fillC);
                bf.Serialize(fs, Shape.lineC);
                fs.Close();
                filename = saveFileDialog1.FileName;
                //string[] filePath = filename.Split('\\');
                //filename = filePath[filePath.Length - 1];
                filechanges = false;
            }
            catch
            {

            }
        }
        void BackState()
        {
            openFileDialog1.Filter = "Binary File(*.bin) | *.bin";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                points = (List<Shape>)bf.Deserialize(fs);
                Shape.R = (int)bf.Deserialize(fs);
                Shape.fillC = (Color)bf.Deserialize(fs);
                Shape.lineC = (Color)bf.Deserialize(fs);
                fs.Close();
                filename = openFileDialog1.FileName;
                Console.WriteLine(filename);
                //string[] filePath = filename.Split('\\');
                //filename = filePath[filePath.Length - 1];
                filechanges = false;
            }
            catch
            {
                Console.WriteLine("что-то пошло не так");
            }
        }
        void newChoice_Click(object sender, EventArgs e)
        {
            //Console.WriteLine(filename);
            //Console.WriteLine(filechanges);
            if (filename != "" && filechanges)
            {
               
                    string message = "Do you want to save changes?";
                    string title = "Save changes";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.Yes)
                    {
                        if (filename == "+")
                        {
                        Console.WriteLine("HERE");
                            SaveState();
                            filename = "";
                            Shape.R = 20;
                            Shape.lineC = Color.OliveDrab;
                            Shape.fillC = Color.YellowGreen;
                            points.Clear();
                            Refresh();
                            filechanges = false;
                        }
                        else
                        {
                            Console.WriteLine("HERE2");
                            BinaryFormatter bf = new BinaryFormatter();
                            FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                            bf.Serialize(fs, points);
                            bf.Serialize(fs, Shape.R);
                            bf.Serialize(fs, Shape.fillC);
                            bf.Serialize(fs, Shape.lineC);
                            fs.Close();
                            filechanges = false;
                            filename = "";
                            Shape.R = 20;
                            Shape.lineC = Color.OliveDrab;
                            Shape.fillC = Color.YellowGreen;
                            points.Clear();
                            Refresh();
                        }
                    }
                    else
                    {
                        Console.WriteLine("HERE3");
                        Console.WriteLine("f: ", filename);
                        filename = "";
                        Shape.R = 20;
                        Shape.lineC = Color.OliveDrab;
                        Shape.fillC = Color.YellowGreen;
                        points.Clear();
                        Refresh();
                    }
            }
            else if (filename != "" && !filechanges)
            {
                filename = "";
                Shape.R = 20;
                Shape.lineC = Color.OliveDrab;
                Shape.fillC = Color.YellowGreen;
                points.Clear();
                Refresh();
            }
            
        }
        void saveasChoice_Click(object sender, EventArgs e)
        {
            SaveState();
        }
        void saveChoice_Click(object sender, EventArgs e)
        {
            Console.WriteLine(filename);
            if (filename == "+" || filename == "")
            {
                SaveState();
            }
            else 
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                bf.Serialize(fs, points);
                bf.Serialize(fs, Shape.R);
                bf.Serialize(fs, Shape.fillC);
                bf.Serialize(fs, Shape.lineC);
                fs.Close();
                filechanges = false;
            }
        }
        void openChoice_Click(object sender, EventArgs e)
        {
            //BackState();
            if (filename != "" && filechanges)
            {
                string message = "Do you want to save changes?";
                string title = "Save changes";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    if (filename == "+")
                    {
                        SaveState();
                        BackState();
                    }
                    else
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        FileStream fs = new FileStream(filename, FileMode.Create, FileAccess.Write);
                        bf.Serialize(fs, points);
                        bf.Serialize(fs, Shape.R);
                        bf.Serialize(fs, Shape.fillC);
                        bf.Serialize(fs, Shape.lineC);
                        fs.Close();
                        BackState();
                    }
                }
                else
                {
                    BackState();
                }
            }
            //else if (filename != "" && !filechanges)
            //{
            //    BackState();
            //}
            else
            {
                BackState();
            }
        }
        void colors_fill_in_CheckedChanged(object sender, EventArgs e)
        {

            if (colorDialog1.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                Color beforeColor = Shape.fillC;
                Shape.fillC = colorDialog1.Color;
                if (beforeColor != Shape.fillC)
                {
                    filechanges = true;
                }
            }
        }
        void colors_line_CheckedChanged(object sender, EventArgs e)
        {

            if (colorDialog2.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                Color beforeColor = Shape.lineC;
                Shape.lineC = colorDialog2.Color;
                if (beforeColor != Shape.lineC)
                {
                    filechanges = true;
                }
            }
        }
        void jarvis_CheckedChanged(object sender, EventArgs e)
        {
            if (jarvisChoice.CheckState == CheckState.Unchecked && simpleChoice.CheckState == CheckState.Unchecked)
            {
                jarvisChoice.CheckState = CheckState.Checked;
            }
            if (simpleChoice.CheckState == CheckState.Checked)
                simpleChoice.CheckState = CheckState.Unchecked;
        }
        void simple_CheckedChanged(object sender, EventArgs e)
        {
            if (simpleChoice.CheckState == CheckState.Unchecked && jarvisChoice.CheckState == CheckState.Unchecked)
            {
                simpleChoice.CheckState = CheckState.Checked;
            }
            if (jarvisChoice.CheckState == CheckState.Checked)
            {
                jarvisChoice.CheckState = CheckState.Unchecked;
            }
        }
        void triangle_CheckedChanged(object sender, EventArgs e)
        {
            if (squareChoice.CheckState == CheckState.Unchecked && circleChoice.CheckState == CheckState.Unchecked && triangleChoice.CheckState == CheckState.Unchecked)
            {
                triangleChoice.CheckState = CheckState.Checked;
            }
            circleChoice.CheckState = CheckState.Unchecked;
            squareChoice.CheckState = CheckState.Unchecked;

        }
        void square_CheckedChanged(object sender, EventArgs e)
        {
            if (squareChoice.CheckState == CheckState.Unchecked && circleChoice.CheckState == CheckState.Unchecked && triangleChoice.CheckState == CheckState.Unchecked)
            {
                squareChoice.CheckState = CheckState.Checked;
            }
            triangleChoice.CheckState = CheckState.Unchecked;
            circleChoice.CheckState = CheckState.Unchecked;
        }
        void circle_CheckedChanged(object sender, EventArgs e)
        {
            if (squareChoice.CheckState == CheckState.Unchecked && circleChoice.CheckState == CheckState.Unchecked && triangleChoice.CheckState == CheckState.Unchecked)
            {
                circleChoice.CheckState = CheckState.Checked;
            }
            triangleChoice.CheckState = CheckState.Unchecked;
            squareChoice.CheckState = CheckState.Unchecked;
        }
        Stack<Change> undo;
        Stack<Change> redo;
        abstract class Change
        {
            public abstract void undo(Stack<Change> stack);
            public abstract void redo(Stack<Change> stack);
        }
        class PointMoveChange : Change
        {
            int n;
            int dx;
            int dy;
            public PointMoveChange(int n, int dx, int dy)
            {
                this.n = n;
                this.dx = dx;
                this.dy = dy;
            }
            public override void undo(Stack<Change> stack)
            {
                stack.Push(this);
            }

            public override void redo(Stack<Change> stack)
            {
                stack.Push(this);
            }
        }
        class FigureMoveChange : Change
        {
            int dx;
            int dy;
            public FigureMoveChange(int ndx, int dy)
            {
                this.dx = dx;
                this.dy = dy;
            }
            public override void undo(Stack<Change> stack)
            {
                stack.Push(this);
            }

            public override void redo(Stack<Change> stack)
            {
                stack.Push(this);
            }
        }
        class PointCreateChange : Change
        {
            int dx, dy;
            public PointCreateChange(int dx, int dy)
            {
                this.dx = dx;
                this.dy = dy;
            }
            public override void undo(Stack<Change> stack)
            {
                stack.Push(this);
            }

            public override void redo(Stack<Change> stack)
            {
                stack.Push(this);
            }
        }
        class PointDeleteChange : Change
        {
            int dx, dy;
            public PointDeleteChange(int dx, int dy)
            {
                this.dx = dx;
                this.dy = dy;
            }
            public override void undo(Stack<Change> stack)
            {
                stack.Push(this);
            }

            public override void redo(Stack<Change> stack)
            {
                stack.Push(this);
            }
        }
        class RadiusChange : Change
        {
            int dr;
            public RadiusChange(int dr)
            {
                this.dr = dr;
            }
            public override void undo(Stack<Change> stack)
            {
                stack.Push(this);
            }

            public override void redo(Stack<Change> stack)
            {
                stack.Push(this);
            }
        }
        class ColorChange : Change
        {
            Color ColorOld, ColorNew;
            public ColorChange(Color ColorOld, Color ColorNew)
            {
                this.ColorOld = ColorOld;
                this.ColorNew = ColorNew;
            }
            public override void undo(Stack<Change> stack)
            {
                stack.Push(this);
            }

            public override void redo(Stack<Change> stack)
            {
                stack.Push(this);
            }
        }
        [Serializable]
        abstract class Shape
        {
            protected int x;
            protected int y;
            public static int R;
            public static Color lineC;
            public static Color fillC;
            [NonSerialized] public static Color connectC;
            [NonSerialized] protected int x_c;
            [NonSerialized] protected int y_c;
            [NonSerialized] protected bool pressed;
            protected bool inline;
            public Shape(int x, int y)
            {
                this.x = x;
                this.y = y;
                pressed = false;
                x_c = 0;
                y_c = 0;
                inline = false;
            }
            static Shape()
            {
                lineC = Color.OliveDrab;
                fillC = Color.YellowGreen;
                R = 20;
            }
            public abstract void Draw(Graphics g);
            public abstract bool IsInside(int xx, int yy);
            public bool Pressed { get; set; }
            public bool Inline
            {
                get
                {
                    return inline;
                }
                set
                {
                    inline = value;
                }
            }
            public int r { get; }
            public int xd
            {
                get
                {
                    return x_c;
                }
                set
                {
                    x_c = value;
                }
            }
            public int yd
            {
                get
                {
                    return y_c;
                }
                set
                {
                    y_c = value;
                }
            }
            public int X
            {
                get { return x; }
                set { x = value; }
            }
            public int Y
            {
                get { return y; }
                set { y = value; }
            }
        }
        [Serializable]
        class Triangle : Shape
        {
            public Triangle(int x, int y) : base(x, y) { }
            Point[] points = new Point[3];
            public override void Draw(Graphics g)
            {
                SolidBrush brush = new SolidBrush(Shape.fillC);
                Pen pen = new Pen(Shape.lineC);
                points[0] = new Point(X, Y - R);
                points[1] = new Point(X - (int)(R * Math.Sqrt(3) / 2), (int)(Y + R / 2));
                points[2] = new Point(X + (int)(R * Math.Sqrt(3) / 2), (int)(Y + R / 2));
                g.FillPolygon(brush, points);
                g.DrawPolygon(pen, points);
            }
            public override bool IsInside(int xx, int yy)
            {
                bool a = (y - R - y - R / 2) * xx + (x - (int)(R * Math.Sqrt(3) / 2) - x) * yy + ((y + R / 2) * x - (x - (int)(R * Math.Sqrt(3) / 2)) * (y - R)) < 0;
                bool b = (y + R / 2 - y - R / 2) * xx + (x + (int)(R * Math.Sqrt(3) / 2) - x + (int)(R * Math.Sqrt(3) / 2)) * yy + ((x - (int)(R * Math.Sqrt(3) / 2)) * (y + R / 2) - (x + (int)(R * Math.Sqrt(3) / 2)) * (y + R / 2)) < 0;
                bool c = (y - R - y - R / 2) * xx + (x + (int)(R * Math.Sqrt(3) / 2) - x) * yy + (x * (y + R / 2) - (y - R) * (x + (int)(R * Math.Sqrt(3) / 2))) > 0;
                return a && b && c;
            }
        }
        [Serializable]
        class Circle : Shape
        {
            public Circle(int x, int y) : base(x, y) { pressed = false; }
            public override void Draw(Graphics g)
            {
                SolidBrush brush = new SolidBrush(Shape.fillC);
                g.FillEllipse(brush, x - R, y - R, 2 * R, 2 * R);
                Pen pen = new Pen(Shape.lineC);
                g.DrawEllipse(pen, x - R, Y - R, 2 * R, 2 * R);
            }
            public override bool IsInside(int xx, int yy)
            {
                int xxx = x;
                int yyy = y;
                return (xxx - xx) * (xxx - xx) + (yyy - yy) * (yyy - yy) <= R * R;
            }
            public bool Pressed
            {
                get { return pressed; }
                set { pressed = value; }
            }
            public int r
            {
                get { return R; }
            }
        }
        [Serializable]
        class Square : Shape
        {
            public Square(int x, int y) : base(x, y) { }
            public override void Draw(Graphics g)
            {
                SolidBrush brush = new SolidBrush(Shape.fillC);
                Pen pen = new Pen(Shape.lineC);
                g.FillRectangle(brush, x - (int)(R / Math.Sqrt(2)), y - (int)(R / Math.Sqrt(2)), (int)(2 * R / Math.Sqrt(2)), (int)(2 * R / Math.Sqrt(2)));
                g.DrawRectangle(pen, x - (int)(R / Math.Sqrt(2)), y - (int)(R / Math.Sqrt(2)), (int)(2 * R / Math.Sqrt(2)), (int)(2 * R / Math.Sqrt(2)));
            }
            public override bool IsInside(int xx, int yy)
            {
                return xx >= x - R && xx <= x - R + (int)(2 * R / Math.Sqrt(2)) && yy >= y - R && yy <= y - R + (int)(2 * R / Math.Sqrt(2));
            }
            public int r
            {
                get { return R; }
            }
            public bool Pressed
            {
                get { return pressed; }
                set { pressed = value; }
            }

        }
        List<Shape> points = new List<Shape>();
        List<int> figure = new List<int>();
        bool figureMove = false;
        void Jarvis(ref List<Shape> points, Graphics g)
        {
            int y_max = 0;
            int x_min = Width + 100;
            int down_index = points.Count - 1;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y > y_max)
                {
                    y_max = points[i].Y;
                    x_min = points[i].X;
                    down_index = i;
                }
                if (points[i].Y == y_max)
                {
                    if (points[i].X < x_min)
                    {
                        x_min = points[i].X;
                        down_index = i;
                    }
                }
            }
            int begin = down_index;
            double cosinus = 2;
            int min = points.Count - 1;
            for (int i = 0; i < points.Count; i++)
            {
                if (i != down_index)
                {
                    double cosin = (10 - points[down_index].X) * (points[i].X - points[down_index].X) / (double)(Math.Sqrt((10 - points[down_index].X) * (10 - points[down_index].X)) * Math.Sqrt((points[i].X - points[down_index].X) * (points[i].X - points[down_index].X) + (points[i].Y - points[down_index].Y) * (points[i].Y - points[down_index].Y)));
                    if (cosin < cosinus)
                    {
                        cosinus = cosin;
                        min = i;
                        figure.Add(i);
                    }
                }
            }
            Pen pen = new Pen(Shape.lineC);
            int start = min;
            int max = points.Count - 1;
            cosinus = 2;
            if (points.Count >= 3)
            {
                g.DrawLine(pen, points[down_index].X, points[down_index].Y, points[min].X, points[min].Y);
                points[down_index].Inline = true;
                points[min].Inline = true;
                while (start != begin)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        int v1_x = points[down_index].X - points[start].X;
                        int v2_x = points[i].X - points[start].X;
                        int v1_y = points[down_index].Y - points[start].Y;
                        int v2_y = points[i].Y - points[start].Y;
                        double v1_length = Math.Sqrt(v1_x * v1_x + v1_y * v1_y);
                        double v2_length = Math.Sqrt(v2_x * v2_x + v2_y * v2_y);
                        double cosin = (v1_x * v2_x + v1_y * v2_y) / (v1_length * v2_length);
                        if (cosin < cosinus)
                        {
                            cosinus = cosin;
                            max = i;
                        }
                    }
                    figure.Add(max);
                    down_index = start;
                    start = max;
                    cosinus = 2;
                    g.DrawLine(pen, points[down_index].X, points[down_index].Y, points[start].X, points[start].Y);
                    points[start].Inline = true;
                }
            }
        }
        void Definition(ref List<Shape> points, Graphics g)
        {
            Pen pen = new Pen(Shape.lineC);
            if (points.Count > 2)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    for (int l = j + 1; l < points.Count; l++)
                    {
                        bool high = true;
                        bool low = true;
                        if (points[j] != points[l] && points[j].X != points[l].X)
                        {
                            double k = (points[j].Y - points[l].Y) / (double)(points[j].X - points[l].X);
                            double b = points[j].Y - k * points[l].X;
                            foreach (Shape f3 in points)
                            {
                                if (f3 != points[j] && points[l] != f3)
                                {
                                    double y1 = (points[j].Y * points[l].X - points[j].X * points[l].Y - (points[j].Y - points[l].Y) * f3.X) / (double)(points[l].X - points[j].X);
                                    if (y1 < f3.Y)
                                    {
                                        low = false;
                                    }
                                    else if (y1 > f3.Y)
                                    {
                                        high = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (Shape f3 in points)
                            {
                                if (f3 != points[j] && points[l] != f3)
                                {
                                    double x1 = -((points[l].X - points[j].X) * points[l].Y + points[j].X * points[l].Y - points[j].Y * points[l].X) / (double)(points[j].Y - points[l].Y);

                                    if (x1 < f3.X)
                                    {
                                        low = false;
                                    }
                                    else if (x1 > f3.X)
                                    {
                                        high = false;
                                    }
                                }
                            }
                        }
                        if (high || low)
                        {
                            points[j].Inline = true;
                            points[l].Inline = true;
                            g.DrawLine(pen, points[j].X, points[j].Y, points[l].X, points[l].Y);
                        }
                    }
                }
            }
        }
        bool testInside(ref List<Shape> points, Shape p)
        {
            int y_max = 0;
            int x_min = Width + 100;
            int down_index = points.Count - 1;

            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y > y_max)
                {
                    y_max = points[i].Y;
                    x_min = points[i].X;
                    down_index = i;
                }
                if (points[i].Y == y_max)
                {
                    if (points[i].X < x_min)
                    {
                        x_min = points[i].X;
                        down_index = i;
                    }
                }
            }
            int begin = down_index;
            double cosinus = 2;
            int min = points.Count - 1;
            for (int i = 0; i < points.Count; i++)
            {
                if (i != down_index)
                {
                    double cosin = (10 - points[down_index].X) * (points[i].X - points[down_index].X) / (double)(Math.Sqrt((10 - points[down_index].X) * (10 - points[down_index].X)) * Math.Sqrt((points[i].X - points[down_index].X) * (points[i].X - points[down_index].X) + (points[i].Y - points[down_index].Y) * (points[i].Y - points[down_index].Y)));
                    if (cosin < cosinus)
                    {
                        cosinus = cosin;
                        min = i;
                        figure.Add(i);
                    }
                }
            }
            Pen pen = new Pen(Color.Black);
            int start = min;
            int max = points.Count - 1;
            cosinus = 2;
            if (points.Count >= 3)
            {
                points[down_index].Inline = true;
                points[min].Inline = true;
                while (start != begin)
                {
                    for (int i = 0; i < points.Count; i++)
                    {
                        int v1_x = points[down_index].X - points[start].X;
                        int v2_x = points[i].X - points[start].X;
                        int v1_y = points[down_index].Y - points[start].Y;
                        int v2_y = points[i].Y - points[start].Y;
                        double v1_length = Math.Sqrt(v1_x * v1_x + v1_y * v1_y);
                        double v2_length = Math.Sqrt(v2_x * v2_x + v2_y * v2_y);
                        double cosin = (v1_x * v2_x + v1_y * v2_y) / (v1_length * v2_length);
                        if (cosin < cosinus)
                        {
                            cosinus = cosin;
                            max = i;
                        }
                    }
                    figure.Add(max);
                    down_index = start;
                    start = max;
                    cosinus = 2;
                    points[start].Inline = true;
                }
            }
            return p.Inline;
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {

            //filechanges = true;
            Graphics g = e.Graphics;
            foreach (Shape f in points)
                f.Inline = false;

            if (jarvisChoice.CheckState == CheckState.Checked)
            {
                Jarvis(ref points, g);
            }
            else
            {
                Definition(ref points, g);
            }

            for (int i = 0; i < points.Count; i++)
            {
                points[i].Draw(g);
                if (filename == "")
                    filename = "+";
            }
        }
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            bool check = false;
            if (e.Button == MouseButtons.Right)
            {
                int i = 0;
                for (i = 0; i < points.Count; i++)
                {
                    if (points[i].IsInside(e.X, e.Y))
                    {
                        filechanges = true;
                        points.RemoveAt(i);
                    }
                }
            }
            else
            {
                foreach (Shape figure in points)
                {
                    if (figure.IsInside(e.X, e.Y))
                    {
                        filechanges = true;
                        figure.Pressed = true;
                        check = true;
                        figure.xd = figure.X - e.X;
                        figure.yd = figure.Y - e.Y;
                    }
                    else figure.Pressed = false;
                }
            }
            if (e.Button == MouseButtons.Left && !check)
            {
                if (points.Count > 2)
                {
                    Shape testFigure;
                    if (squareChoice.CheckState == CheckState.Checked)
                        testFigure = new Square(e.X, e.Y);
                    else if (circleChoice.CheckState == CheckState.Checked)
                        testFigure = new Circle(e.X, e.Y);
                    else
                        testFigure = new Triangle(e.X, e.Y);
                    filechanges = true;
                    points.Add(testFigure);
                    figureMove = !testInside(ref points, testFigure); //потому что если у точки isInside = false, то мы можем двигать фигуру, и наоборот
                    if (figureMove)
                    {
                        points.Remove(testFigure);
                        this.Refresh();
                    }
                }
                if (figureMove)
                {
                    foreach (Shape p in points)
                    {
                        p.xd = p.X - e.X;
                        p.yd = p.Y - e.Y;
                    }
                }
                Shape figure;
                if (squareChoice.CheckState == CheckState.Checked)
                    figure = new Square(e.X, e.Y);
                else if (circleChoice.CheckState == CheckState.Checked)
                    figure = new Circle(e.X, e.Y);
                else
                    figure = new Triangle(e.X, e.Y);
                if (!figureMove)
                {
                    points.Add(figure);
                    filechanges = true;
                }
            }
            this.Refresh();
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            figureMove = false;
            for (int i = 0; i < points.Count; i++)
            {
                points[i].Pressed = false;
                if (points.Count > 2 && points[i].Inline == false)
                {
                    points.RemoveAt(i);
                    i--;
                }
            }
            this.Refresh();
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            foreach (Shape figure in points)
            {
                if (figure.Pressed)
                {
                    figure.X = e.X + figure.xd;
                    figure.Y = e.Y + figure.yd;
                    filechanges = true;
                }
            }
            if (figureMove)
            {
                foreach (Shape p in points)
                {
                    p.X = e.X + p.xd;
                    p.Y = e.Y + p.yd;
                    filechanges = true;
                }
            }
            this.Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        void radiusChoice_Click(object sender, EventArgs e)
        {
            if (f2 == null || f2.IsDisposed)
            {
                f2 = new Form2();
                f2.Show();
            }
            else
            {
                if (f2.WindowState == FormWindowState.Normal)
                {
                    f2.Activate();
                }
                else if (f2.WindowState == FormWindowState.Minimized)
                {
                    f2.WindowState = FormWindowState.Normal;
                }
                else
                {
                    Console.WriteLine("YESS");
                }
                //открыта
                //закрыта
                //свернута
            }
            //f2.WindowState
            if (f2 != null)
                f2.RC += new RadiusChanged(onRadiusChanged);
        }
        public void onRadiusChanged(object sender, RadiusEventArgs e)
        {
            Shape.R = e.Radius;
            filechanges = true;
            Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!figureMove)
            {
                int timer_interval = timer1.Interval;
                try
                {
                    timer1.Interval = int.Parse(textBox1.Text);
                }
                catch
                {
                    timer1.Interval = timer_interval;
                }
                Random random = new Random();
                for (int i = 0; i < points.Count; i++)
                {
                    points[i].X += random.Next(-1, 2);
                    points[i].Y += random.Next(-1, 2);
                    if (testInside(ref points, points[i]) == false)
                    {
                        points.RemoveAt(i);
                        i--;
                    }
                }
                this.Refresh();
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}

