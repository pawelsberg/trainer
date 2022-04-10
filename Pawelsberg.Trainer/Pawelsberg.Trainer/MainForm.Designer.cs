namespace Pawelsberg.Trainer
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea4 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend4 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.openFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intervalsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphDistanceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.graphDurationMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.valueMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.speedUnitsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapUnZoomMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapPictureBox = new System.Windows.Forms.PictureBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.workoutsTabControl = new System.Windows.Forms.TabControl();
            this.workoutListTabPage = new System.Windows.Forms.TabPage();
            this.listView2 = new System.Windows.Forms.ListView();
            this.workoutCalendarTabPage = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.monthCalendar1 = new System.Windows.Forms.MonthCalendar();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.splitter3 = new System.Windows.Forms.Splitter();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.splitter2 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).BeginInit();
            this.workoutsTabControl.SuspendLayout();
            this.workoutListTabPage.SuspendLayout();
            this.workoutCalendarTabPage.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea4.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea4);
            this.chart1.Dock = System.Windows.Forms.DockStyle.Top;
            legend4.Enabled = false;
            legend4.Name = "Legend1";
            this.chart1.Legends.Add(legend4);
            this.chart1.Location = new System.Drawing.Point(0, 24);
            this.chart1.Name = "chart1";
            series4.ChartArea = "ChartArea1";
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            this.chart1.Series.Add(series4);
            this.chart1.Size = new System.Drawing.Size(546, 135);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            this.chart1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart1_MouseMove);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openFolderMenuItem,
            this.intervalsMenuItem,
            this.graphDistanceMenuItem,
            this.graphDurationMenuItem,
            this.valueMenuItem,
            this.speedUnitsMenuItem,
            this.mapZoomMenuItem,
            this.mapUnZoomMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(920, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // openFolderMenuItem
            // 
            this.openFolderMenuItem.Name = "openFolderMenuItem";
            this.openFolderMenuItem.Size = new System.Drawing.Size(84, 20);
            this.openFolderMenuItem.Text = "Open Folder";
            // 
            // intervalsMenuItem
            // 
            this.intervalsMenuItem.Name = "intervalsMenuItem";
            this.intervalsMenuItem.Size = new System.Drawing.Size(63, 20);
            this.intervalsMenuItem.Text = "Intervals";
            // 
            // graphDistanceMenuItem
            // 
            this.graphDistanceMenuItem.Name = "graphDistanceMenuItem";
            this.graphDistanceMenuItem.Size = new System.Drawing.Size(107, 20);
            this.graphDistanceMenuItem.Text = "Graph - Distance";
            // 
            // graphDurationMenuItem
            // 
            this.graphDurationMenuItem.Name = "graphDurationMenuItem";
            this.graphDurationMenuItem.Size = new System.Drawing.Size(88, 20);
            this.graphDurationMenuItem.Text = "Graph - Time";
            // 
            // valueMenuItem
            // 
            this.valueMenuItem.Name = "valueMenuItem";
            this.valueMenuItem.Size = new System.Drawing.Size(12, 20);
            // 
            // speedUnitsMenuItem
            // 
            this.speedUnitsMenuItem.Name = "speedUnitsMenuItem";
            this.speedUnitsMenuItem.Size = new System.Drawing.Size(70, 20);
            this.speedUnitsMenuItem.Text = "[min/km]";
            // 
            // mapZoomMenuItem
            // 
            this.mapZoomMenuItem.Name = "mapZoomMenuItem";
            this.mapZoomMenuItem.Size = new System.Drawing.Size(78, 20);
            this.mapZoomMenuItem.Text = "Map Zoom";
            // 
            // mapUnZoomMenuItem
            // 
            this.mapUnZoomMenuItem.Name = "mapUnZoomMenuItem";
            this.mapUnZoomMenuItem.Size = new System.Drawing.Size(93, 20);
            this.mapUnZoomMenuItem.Text = "Map UnZoom";
            // 
            // mapPictureBox
            // 
            this.mapPictureBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mapPictureBox.Location = new System.Drawing.Point(0, 0);
            this.mapPictureBox.Name = "mapPictureBox";
            this.mapPictureBox.Size = new System.Drawing.Size(543, 124);
            this.mapPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.mapPictureBox.TabIndex = 2;
            this.mapPictureBox.TabStop = false;
            this.mapPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.mapPictureBox_MouseDown);
            this.mapPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mapPictureBox_MouseMove);
            this.mapPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.mapPictureBox_MouseUp);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 159);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(546, 3);
            this.splitter1.TabIndex = 3;
            this.splitter1.TabStop = false;
            // 
            // workoutsTabControl
            // 
            this.workoutsTabControl.Controls.Add(this.workoutListTabPage);
            this.workoutsTabControl.Controls.Add(this.workoutCalendarTabPage);
            this.workoutsTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.workoutsTabControl.Location = new System.Drawing.Point(3, 16);
            this.workoutsTabControl.Name = "workoutsTabControl";
            this.workoutsTabControl.SelectedIndex = 0;
            this.workoutsTabControl.Size = new System.Drawing.Size(234, 113);
            this.workoutsTabControl.TabIndex = 4;
            // 
            // workoutListTabPage
            // 
            this.workoutListTabPage.Controls.Add(this.listView2);
            this.workoutListTabPage.Location = new System.Drawing.Point(4, 22);
            this.workoutListTabPage.Name = "workoutListTabPage";
            this.workoutListTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.workoutListTabPage.Size = new System.Drawing.Size(226, 87);
            this.workoutListTabPage.TabIndex = 0;
            this.workoutListTabPage.Text = "List";
            this.workoutListTabPage.UseVisualStyleBackColor = true;
            // 
            // listView2
            // 
            this.listView2.Alignment = System.Windows.Forms.ListViewAlignment.Left;
            this.listView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView2.Location = new System.Drawing.Point(3, 3);
            this.listView2.MultiSelect = false;
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(220, 81);
            this.listView2.TabIndex = 0;
            this.listView2.UseCompatibleStateImageBehavior = false;
            this.listView2.View = System.Windows.Forms.View.List;
            this.listView2.ItemActivate += new System.EventHandler(this.listView2_ItemActivate);
            // 
            // workoutCalendarTabPage
            // 
            this.workoutCalendarTabPage.Controls.Add(this.listView1);
            this.workoutCalendarTabPage.Controls.Add(this.monthCalendar1);
            this.workoutCalendarTabPage.Location = new System.Drawing.Point(4, 22);
            this.workoutCalendarTabPage.Name = "workoutCalendarTabPage";
            this.workoutCalendarTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.workoutCalendarTabPage.Size = new System.Drawing.Size(226, 87);
            this.workoutCalendarTabPage.TabIndex = 1;
            this.workoutCalendarTabPage.Text = "Calendar";
            this.workoutCalendarTabPage.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(3, 165);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(220, 0);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            this.listView1.ItemActivate += new System.EventHandler(this.listView2_ItemActivate);
            // 
            // monthCalendar1
            // 
            this.monthCalendar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.monthCalendar1.Location = new System.Drawing.Point(3, 3);
            this.monthCalendar1.MaxSelectionCount = 1;
            this.monthCalendar1.Name = "monthCalendar1";
            this.monthCalendar1.ShowWeekNumbers = true;
            this.monthCalendar1.TabIndex = 0;
            this.monthCalendar1.DateSelected += new System.Windows.Forms.DateRangeEventHandler(this.monthCalendar1_DateSelected);
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.propertyGrid1.Location = new System.Drawing.Point(3, 129);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(234, 100);
            this.propertyGrid1.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.splitter3);
            this.groupBox1.Controls.Add(this.workoutsTabControl);
            this.groupBox1.Controls.Add(this.propertyGrid1);
            this.groupBox1.Controls.Add(this.webBrowser1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(549, 24);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(240, 444);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Workouts";
            // 
            // splitter3
            // 
            this.splitter3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter3.Location = new System.Drawing.Point(3, 126);
            this.splitter3.Name = "splitter3";
            this.splitter3.Size = new System.Drawing.Size(234, 3);
            this.splitter3.TabIndex = 5;
            this.splitter3.TabStop = false;
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.webBrowser1.Location = new System.Drawing.Point(3, 229);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(234, 212);
            this.webBrowser1.TabIndex = 6;
            this.webBrowser1.Navigating += new System.Windows.Forms.WebBrowserNavigatingEventHandler(this.webBrowser1_Navigating);
            // 
            // splitter2
            // 
            this.splitter2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.splitter2.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter2.Location = new System.Drawing.Point(546, 24);
            this.splitter2.Name = "splitter2";
            this.splitter2.Size = new System.Drawing.Size(3, 444);
            this.splitter2.TabIndex = 7;
            this.splitter2.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.mapPictureBox);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 162);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(546, 306);
            this.panel1.TabIndex = 9;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(789, 468);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.chart1);
            this.Controls.Add(this.splitter2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Trainer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mapPictureBox)).EndInit();
            this.workoutsTabControl.ResumeLayout(false);
            this.workoutListTabPage.ResumeLayout(false);
            this.workoutCalendarTabPage.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem intervalsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem graphDistanceMenuItem;
        private System.Windows.Forms.ToolStripMenuItem valueMenuItem;
        private System.Windows.Forms.PictureBox mapPictureBox;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TabControl workoutsTabControl;
        private System.Windows.Forms.TabPage workoutListTabPage;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.TabPage workoutCalendarTabPage;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.MonthCalendar monthCalendar1;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Splitter splitter3;
        private System.Windows.Forms.Splitter splitter2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.WebBrowser webBrowser1;
        private System.Windows.Forms.ToolStripMenuItem graphDurationMenuItem;
        private System.Windows.Forms.ToolStripMenuItem speedUnitsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapZoomMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mapUnZoomMenuItem;
        private ToolStripMenuItem openFolderMenuItem;
    }
}

