namespace Patchlunky
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblModName = new System.Windows.Forms.Label();
            this.picModImage = new System.Windows.Forms.PictureBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cboConfig = new System.Windows.Forms.ComboBox();
            this.lblModText = new System.Windows.Forms.Label();
            this.lblModList = new System.Windows.Forms.Label();
            this.lblLoadOrder = new System.Windows.Forms.Label();
            this.txtModInfo = new System.Windows.Forms.TextBox();
            this.lblSelect = new System.Windows.Forms.Label();
            this.lblMove = new System.Windows.Forms.Label();
            this.btnBottom = new System.Windows.Forms.Button();
            this.btnTop = new System.Windows.Forms.Button();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.chklstMods = new System.Windows.Forms.CheckedListBox();
            this.btnModDown = new System.Windows.Forms.Button();
            this.btnModUp = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblSkin = new System.Windows.Forms.Label();
            this.btnRestoreSkin = new System.Windows.Forms.Button();
            this.lblSkinSelect = new System.Windows.Forms.Label();
            this.lblCharSelect = new System.Windows.Forms.Label();
            this.lblCharacter = new System.Windows.Forms.Label();
            this.picSkin = new System.Windows.Forms.PictureBox();
            this.lvwSkins = new System.Windows.Forms.ListView();
            this.btnResetSkins = new System.Windows.Forms.Button();
            this.picCharacter = new System.Windows.Forms.PictureBox();
            this.lvwCharacters = new System.Windows.Forms.ListView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnExtractArchives = new System.Windows.Forms.Button();
            this.btnRestore = new System.Windows.Forms.Button();
            this.btnSetup = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblStatusBackup = new System.Windows.Forms.Label();
            this.lblStatusGamePath = new System.Windows.Forms.Label();
            this.btnGamePath = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGamePath = new System.Windows.Forms.TextBox();
            this.prgPatchGame = new System.Windows.Forms.ProgressBar();
            this.btnPatch = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.btnRunGame = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picModImage)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSkin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).BeginInit();
            this.tabPage3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(673, 413);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblModName);
            this.tabPage1.Controls.Add(this.picModImage);
            this.tabPage1.Controls.Add(this.btnDelete);
            this.tabPage1.Controls.Add(this.btnSave);
            this.tabPage1.Controls.Add(this.cboConfig);
            this.tabPage1.Controls.Add(this.lblModText);
            this.tabPage1.Controls.Add(this.lblModList);
            this.tabPage1.Controls.Add(this.lblLoadOrder);
            this.tabPage1.Controls.Add(this.txtModInfo);
            this.tabPage1.Controls.Add(this.lblSelect);
            this.tabPage1.Controls.Add(this.lblMove);
            this.tabPage1.Controls.Add(this.btnBottom);
            this.tabPage1.Controls.Add(this.btnTop);
            this.tabPage1.Controls.Add(this.btnAll);
            this.tabPage1.Controls.Add(this.btnNone);
            this.tabPage1.Controls.Add(this.chklstMods);
            this.tabPage1.Controls.Add(this.btnModDown);
            this.tabPage1.Controls.Add(this.btnModUp);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(665, 387);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Mods";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblModName
            // 
            this.lblModName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblModName.Location = new System.Drawing.Point(401, 22);
            this.lblModName.Name = "lblModName";
            this.lblModName.Size = new System.Drawing.Size(256, 19);
            this.lblModName.TabIndex = 24;
            // 
            // picModImage
            // 
            this.picModImage.BackColor = System.Drawing.Color.Black;
            this.picModImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picModImage.Location = new System.Drawing.Point(401, 46);
            this.picModImage.Name = "picModImage";
            this.picModImage.Size = new System.Drawing.Size(256, 144);
            this.picModImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picModImage.TabIndex = 23;
            this.picModImage.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(288, 7);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(49, 21);
            this.btnDelete.TabIndex = 22;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(238, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(44, 21);
            this.btnSave.TabIndex = 21;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cboConfig
            // 
            this.cboConfig.FormattingEnabled = true;
            this.cboConfig.Location = new System.Drawing.Point(8, 7);
            this.cboConfig.Name = "cboConfig";
            this.cboConfig.Size = new System.Drawing.Size(224, 21);
            this.cboConfig.TabIndex = 19;
            this.cboConfig.Text = "Default";
            this.cboConfig.SelectedIndexChanged += new System.EventHandler(this.cboConfig_SelectedIndexChanged);
            // 
            // lblModText
            // 
            this.lblModText.AutoSize = true;
            this.lblModText.Location = new System.Drawing.Point(398, 7);
            this.lblModText.Name = "lblModText";
            this.lblModText.Size = new System.Drawing.Size(75, 13);
            this.lblModText.TabIndex = 18;
            this.lblModText.Text = "Selected mod:";
            // 
            // lblModList
            // 
            this.lblModList.AutoSize = true;
            this.lblModList.Location = new System.Drawing.Point(5, 31);
            this.lblModList.Name = "lblModList";
            this.lblModList.Size = new System.Drawing.Size(169, 13);
            this.lblModList.TabIndex = 17;
            this.lblModList.Text = "Select mods to patch to the game:";
            // 
            // lblLoadOrder
            // 
            this.lblLoadOrder.AutoSize = true;
            this.lblLoadOrder.Location = new System.Drawing.Point(8, 368);
            this.lblLoadOrder.Name = "lblLoadOrder";
            this.lblLoadOrder.Size = new System.Drawing.Size(238, 13);
            this.lblLoadOrder.TabIndex = 16;
            this.lblLoadOrder.Text = "Mods higher in the list override mods below them.";
            // 
            // txtModInfo
            // 
            this.txtModInfo.Location = new System.Drawing.Point(401, 196);
            this.txtModInfo.Multiline = true;
            this.txtModInfo.Name = "txtModInfo";
            this.txtModInfo.ReadOnly = true;
            this.txtModInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtModInfo.Size = new System.Drawing.Size(256, 169);
            this.txtModInfo.TabIndex = 15;
            // 
            // lblSelect
            // 
            this.lblSelect.AutoSize = true;
            this.lblSelect.Location = new System.Drawing.Point(341, 273);
            this.lblSelect.Name = "lblSelect";
            this.lblSelect.Size = new System.Drawing.Size(40, 13);
            this.lblSelect.TabIndex = 14;
            this.lblSelect.Text = "Select:";
            // 
            // lblMove
            // 
            this.lblMove.AutoSize = true;
            this.lblMove.Location = new System.Drawing.Point(341, 57);
            this.lblMove.Name = "lblMove";
            this.lblMove.Size = new System.Drawing.Size(37, 13);
            this.lblMove.TabIndex = 13;
            this.lblMove.Text = "Move:";
            // 
            // btnBottom
            // 
            this.btnBottom.Location = new System.Drawing.Point(344, 196);
            this.btnBottom.Name = "btnBottom";
            this.btnBottom.Size = new System.Drawing.Size(49, 35);
            this.btnBottom.TabIndex = 12;
            this.btnBottom.Text = "Bottom";
            this.btnBottom.UseVisualStyleBackColor = true;
            this.btnBottom.Click += new System.EventHandler(this.btnBottom_Click);
            // 
            // btnTop
            // 
            this.btnTop.Location = new System.Drawing.Point(344, 73);
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new System.Drawing.Size(49, 35);
            this.btnTop.TabIndex = 11;
            this.btnTop.Text = "Top";
            this.btnTop.UseVisualStyleBackColor = true;
            this.btnTop.Click += new System.EventHandler(this.btnTop_Click);
            // 
            // btnAll
            // 
            this.btnAll.Location = new System.Drawing.Point(344, 289);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(49, 35);
            this.btnAll.TabIndex = 10;
            this.btnAll.Text = "All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnNone
            // 
            this.btnNone.Location = new System.Drawing.Point(344, 330);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(49, 35);
            this.btnNone.TabIndex = 9;
            this.btnNone.Text = "None";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // chklstMods
            // 
            this.chklstMods.FormattingEnabled = true;
            this.chklstMods.Location = new System.Drawing.Point(8, 46);
            this.chklstMods.Name = "chklstMods";
            this.chklstMods.Size = new System.Drawing.Size(329, 319);
            this.chklstMods.TabIndex = 3;
            this.chklstMods.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.chklstMods_ItemCheck);
            this.chklstMods.SelectedIndexChanged += new System.EventHandler(this.chklstMods_SelectedIndexChanged);
            // 
            // btnModDown
            // 
            this.btnModDown.Location = new System.Drawing.Point(344, 155);
            this.btnModDown.Name = "btnModDown";
            this.btnModDown.Size = new System.Drawing.Size(49, 35);
            this.btnModDown.TabIndex = 2;
            this.btnModDown.Text = "Down";
            this.btnModDown.UseVisualStyleBackColor = true;
            this.btnModDown.Click += new System.EventHandler(this.btnModDown_Click);
            // 
            // btnModUp
            // 
            this.btnModUp.Location = new System.Drawing.Point(344, 114);
            this.btnModUp.Name = "btnModUp";
            this.btnModUp.Size = new System.Drawing.Size(49, 35);
            this.btnModUp.TabIndex = 1;
            this.btnModUp.Text = "Up";
            this.btnModUp.UseVisualStyleBackColor = true;
            this.btnModUp.Click += new System.EventHandler(this.btnModUp_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblSkin);
            this.tabPage2.Controls.Add(this.btnRestoreSkin);
            this.tabPage2.Controls.Add(this.lblSkinSelect);
            this.tabPage2.Controls.Add(this.lblCharSelect);
            this.tabPage2.Controls.Add(this.lblCharacter);
            this.tabPage2.Controls.Add(this.picSkin);
            this.tabPage2.Controls.Add(this.lvwSkins);
            this.tabPage2.Controls.Add(this.btnResetSkins);
            this.tabPage2.Controls.Add(this.picCharacter);
            this.tabPage2.Controls.Add(this.lvwCharacters);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(665, 387);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Character Skins";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblSkin
            // 
            this.lblSkin.AutoSize = true;
            this.lblSkin.Location = new System.Drawing.Point(592, 40);
            this.lblSkin.Name = "lblSkin";
            this.lblSkin.Size = new System.Drawing.Size(31, 13);
            this.lblSkin.TabIndex = 13;
            this.lblSkin.Text = "Skin:";
            // 
            // btnRestoreSkin
            // 
            this.btnRestoreSkin.Location = new System.Drawing.Point(449, 277);
            this.btnRestoreSkin.Name = "btnRestoreSkin";
            this.btnRestoreSkin.Size = new System.Drawing.Size(206, 36);
            this.btnRestoreSkin.TabIndex = 12;
            this.btnRestoreSkin.Text = "Reset selected character to default";
            this.btnRestoreSkin.UseVisualStyleBackColor = true;
            this.btnRestoreSkin.Click += new System.EventHandler(this.btnRestoreSkin_Click);
            // 
            // lblSkinSelect
            // 
            this.lblSkinSelect.AutoSize = true;
            this.lblSkinSelect.Location = new System.Drawing.Point(8, 6);
            this.lblSkinSelect.Name = "lblSkinSelect";
            this.lblSkinSelect.Size = new System.Drawing.Size(151, 13);
            this.lblSkinSelect.TabIndex = 11;
            this.lblSkinSelect.Text = "Double-click a skin to select it:";
            this.lblSkinSelect.Visible = false;
            // 
            // lblCharSelect
            // 
            this.lblCharSelect.AutoSize = true;
            this.lblCharSelect.Location = new System.Drawing.Point(8, 6);
            this.lblCharSelect.Name = "lblCharSelect";
            this.lblCharSelect.Size = new System.Drawing.Size(212, 13);
            this.lblCharSelect.TabIndex = 10;
            this.lblCharSelect.Text = "Double-click a character to change its skin:";
            // 
            // lblCharacter
            // 
            this.lblCharacter.AutoSize = true;
            this.lblCharacter.Location = new System.Drawing.Point(458, 31);
            this.lblCharacter.Name = "lblCharacter";
            this.lblCharacter.Size = new System.Drawing.Size(100, 13);
            this.lblCharacter.TabIndex = 9;
            this.lblCharacter.Text = "Selected character:";
            // 
            // picSkin
            // 
            this.picSkin.BackColor = System.Drawing.Color.AliceBlue;
            this.picSkin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSkin.Location = new System.Drawing.Point(567, 56);
            this.picSkin.Name = "picSkin";
            this.picSkin.Size = new System.Drawing.Size(80, 80);
            this.picSkin.TabIndex = 8;
            this.picSkin.TabStop = false;
            // 
            // lvwSkins
            // 
            this.lvwSkins.BackColor = System.Drawing.Color.AliceBlue;
            this.lvwSkins.HideSelection = false;
            this.lvwSkins.Location = new System.Drawing.Point(8, 27);
            this.lvwSkins.MultiSelect = false;
            this.lvwSkins.Name = "lvwSkins";
            this.lvwSkins.Size = new System.Drawing.Size(435, 328);
            this.lvwSkins.TabIndex = 7;
            this.lvwSkins.TileSize = new System.Drawing.Size(80, 80);
            this.lvwSkins.UseCompatibleStateImageBehavior = false;
            this.lvwSkins.View = System.Windows.Forms.View.Tile;
            this.lvwSkins.Visible = false;
            this.lvwSkins.SelectedIndexChanged += new System.EventHandler(this.lvwSkins_SelectedIndexChanged);
            this.lvwSkins.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwSkins_MouseDoubleClick);
            // 
            // btnResetSkins
            // 
            this.btnResetSkins.Location = new System.Drawing.Point(449, 319);
            this.btnResetSkins.Name = "btnResetSkins";
            this.btnResetSkins.Size = new System.Drawing.Size(206, 36);
            this.btnResetSkins.TabIndex = 6;
            this.btnResetSkins.Text = "Reset ALL characters to default";
            this.btnResetSkins.UseVisualStyleBackColor = true;
            this.btnResetSkins.Click += new System.EventHandler(this.btnResetSkins_Click);
            // 
            // picCharacter
            // 
            this.picCharacter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.picCharacter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCharacter.Location = new System.Drawing.Point(461, 47);
            this.picCharacter.Name = "picCharacter";
            this.picCharacter.Size = new System.Drawing.Size(100, 100);
            this.picCharacter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCharacter.TabIndex = 5;
            this.picCharacter.TabStop = false;
            // 
            // lvwCharacters
            // 
            this.lvwCharacters.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvwCharacters.HideSelection = false;
            this.lvwCharacters.Location = new System.Drawing.Point(8, 27);
            this.lvwCharacters.MultiSelect = false;
            this.lvwCharacters.Name = "lvwCharacters";
            this.lvwCharacters.Size = new System.Drawing.Size(435, 328);
            this.lvwCharacters.TabIndex = 4;
            this.lvwCharacters.TileSize = new System.Drawing.Size(80, 80);
            this.lvwCharacters.UseCompatibleStateImageBehavior = false;
            this.lvwCharacters.View = System.Windows.Forms.View.Tile;
            this.lvwCharacters.SelectedIndexChanged += new System.EventHandler(this.lvwCharacters_SelectedIndexChanged);
            this.lvwCharacters.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwCharacters_MouseDoubleClick);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label2);
            this.tabPage3.Controls.Add(this.pictureBox1);
            this.tabPage3.Controls.Add(this.btnExtractArchives);
            this.tabPage3.Controls.Add(this.btnRestore);
            this.tabPage3.Controls.Add(this.btnSetup);
            this.tabPage3.Controls.Add(this.groupBox1);
            this.tabPage3.Controls.Add(this.btnGamePath);
            this.tabPage3.Controls.Add(this.label1);
            this.tabPage3.Controls.Add(this.txtGamePath);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(665, 387);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Miriam", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(112, 224);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 32);
            this.label2.TabIndex = 10;
            this.label2.Text = "Under construction.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Patchlunky.Properties.Resources.under_construction;
            this.pictureBox1.Location = new System.Drawing.Point(27, 211);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(64, 64);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // btnExtractArchives
            // 
            this.btnExtractArchives.Location = new System.Drawing.Point(572, 16);
            this.btnExtractArchives.Name = "btnExtractArchives";
            this.btnExtractArchives.Size = new System.Drawing.Size(83, 59);
            this.btnExtractArchives.TabIndex = 8;
            this.btnExtractArchives.Text = "MOD-UTIL: Extract default archives";
            this.toolTip1.SetToolTip(this.btnExtractArchives, "This extracts alltex.wad and allsounds.wad contents to Patchlunky_Temp/. Mod auth" +
                    "ors may find this useful.");
            this.btnExtractArchives.UseVisualStyleBackColor = true;
            this.btnExtractArchives.Click += new System.EventHandler(this.btnExtractArchives_Click);
            // 
            // btnRestore
            // 
            this.btnRestore.Location = new System.Drawing.Point(291, 16);
            this.btnRestore.Name = "btnRestore";
            this.btnRestore.Size = new System.Drawing.Size(94, 60);
            this.btnRestore.TabIndex = 7;
            this.btnRestore.Text = "Restore default game files";
            this.btnRestore.UseVisualStyleBackColor = true;
            this.btnRestore.Click += new System.EventHandler(this.btnRestore_Click);
            // 
            // btnSetup
            // 
            this.btnSetup.Location = new System.Drawing.Point(186, 16);
            this.btnSetup.Name = "btnSetup";
            this.btnSetup.Size = new System.Drawing.Size(99, 60);
            this.btnSetup.TabIndex = 6;
            this.btnSetup.Text = "Setup Patchlunky";
            this.btnSetup.UseVisualStyleBackColor = true;
            this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblStatusBackup);
            this.groupBox1.Controls.Add(this.lblStatusGamePath);
            this.groupBox1.Location = new System.Drawing.Point(8, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(162, 65);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Patchlunky Status";
            // 
            // lblStatusBackup
            // 
            this.lblStatusBackup.AutoSize = true;
            this.lblStatusBackup.Location = new System.Drawing.Point(6, 35);
            this.lblStatusBackup.Name = "lblStatusBackup";
            this.lblStatusBackup.Size = new System.Drawing.Size(108, 13);
            this.lblStatusBackup.TabIndex = 2;
            this.lblStatusBackup.Text = "Game backups : N/A";
            // 
            // lblStatusGamePath
            // 
            this.lblStatusGamePath.AutoSize = true;
            this.lblStatusGamePath.Location = new System.Drawing.Point(6, 18);
            this.lblStatusGamePath.Name = "lblStatusGamePath";
            this.lblStatusGamePath.Size = new System.Drawing.Size(85, 13);
            this.lblStatusGamePath.TabIndex = 1;
            this.lblStatusGamePath.Text = "Game path: N/A";
            // 
            // btnGamePath
            // 
            this.btnGamePath.Location = new System.Drawing.Point(578, 98);
            this.btnGamePath.Name = "btnGamePath";
            this.btnGamePath.Size = new System.Drawing.Size(77, 22);
            this.btnGamePath.TabIndex = 2;
            this.btnGamePath.Text = "Browse";
            this.btnGamePath.UseVisualStyleBackColor = true;
            this.btnGamePath.Click += new System.EventHandler(this.btnGamePath_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Path to Spelunky:";
            // 
            // txtGamePath
            // 
            this.txtGamePath.BackColor = System.Drawing.SystemColors.ControlLight;
            this.txtGamePath.Location = new System.Drawing.Point(13, 99);
            this.txtGamePath.Name = "txtGamePath";
            this.txtGamePath.Size = new System.Drawing.Size(559, 20);
            this.txtGamePath.TabIndex = 0;
            this.txtGamePath.Leave += new System.EventHandler(this.txtGamePath_Leave);
            // 
            // prgPatchGame
            // 
            this.prgPatchGame.Location = new System.Drawing.Point(95, 423);
            this.prgPatchGame.Name = "prgPatchGame";
            this.prgPatchGame.Size = new System.Drawing.Size(466, 23);
            this.prgPatchGame.TabIndex = 23;
            // 
            // btnPatch
            // 
            this.btnPatch.Location = new System.Drawing.Point(4, 416);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(83, 35);
            this.btnPatch.TabIndex = 8;
            this.btnPatch.Text = "Patch Game";
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(4, 457);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(662, 102);
            this.txtLog.TabIndex = 4;
            this.txtLog.WordWrap = false;
            // 
            // btnRunGame
            // 
            this.btnRunGame.Location = new System.Drawing.Point(570, 419);
            this.btnRunGame.Name = "btnRunGame";
            this.btnRunGame.Size = new System.Drawing.Size(96, 32);
            this.btnRunGame.TabIndex = 24;
            this.btnRunGame.Text = "Launch Game";
            this.btnRunGame.UseVisualStyleBackColor = true;
            this.btnRunGame.Click += new System.EventHandler(this.btnRunGame_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 8000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 565);
            this.Controls.Add(this.btnRunGame);
            this.Controls.Add(this.prgPatchGame);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnPatch);
            this.Controls.Add(this.txtLog);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Patchlunky";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picModImage)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSkin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button btnGamePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtGamePath;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblStatusBackup;
        private System.Windows.Forms.Label lblStatusGamePath;
        private System.Windows.Forms.Button btnSetup;
        private System.Windows.Forms.Button btnModDown;
        private System.Windows.Forms.Button btnModUp;
        private System.Windows.Forms.CheckedListBox chklstMods;
        private System.Windows.Forms.Button btnPatch;
        private System.Windows.Forms.Button btnRestore;
        private System.Windows.Forms.Button btnExtractArchives;
        private System.Windows.Forms.TextBox txtModInfo;
        private System.Windows.Forms.Label lblSelect;
        private System.Windows.Forms.Label lblMove;
        private System.Windows.Forms.Button btnBottom;
        private System.Windows.Forms.Button btnTop;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnNone;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cboConfig;
        private System.Windows.Forms.Label lblModText;
        private System.Windows.Forms.Label lblModList;
        private System.Windows.Forms.Label lblLoadOrder;
        private System.Windows.Forms.ProgressBar prgPatchGame;
        private System.Windows.Forms.Button btnRunGame;
        private System.Windows.Forms.PictureBox picModImage;
        private System.Windows.Forms.Label lblModName;
        private System.Windows.Forms.ListView lvwCharacters;
        private System.Windows.Forms.Button btnResetSkins;
        private System.Windows.Forms.PictureBox picCharacter;
        private System.Windows.Forms.ListView lvwSkins;
        private System.Windows.Forms.PictureBox picSkin;
        private System.Windows.Forms.Label lblCharacter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblSkinSelect;
        private System.Windows.Forms.Label lblCharSelect;
        private System.Windows.Forms.Button btnRestoreSkin;
        private System.Windows.Forms.Label lblSkin;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}

