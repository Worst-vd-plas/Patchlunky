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
            this.btnDeleteModConfig = new System.Windows.Forms.Button();
            this.btnSaveModConfig = new System.Windows.Forms.Button();
            this.cboModConfig = new System.Windows.Forms.ComboBox();
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
            this.lblSkinFiller = new System.Windows.Forms.Label();
            this.lnkSkinEmail = new System.Windows.Forms.LinkLabel();
            this.lnkSkinWebUrl = new System.Windows.Forms.LinkLabel();
            this.lblSkinList = new System.Windows.Forms.Label();
            this.txtSkinInfo = new System.Windows.Forms.TextBox();
            this.lblSkinText = new System.Windows.Forms.Label();
            this.btnRestoreSkin = new System.Windows.Forms.Button();
            this.picCharacter = new System.Windows.Forms.PictureBox();
            this.picSkin = new System.Windows.Forms.PictureBox();
            this.lblCharacter = new System.Windows.Forms.Label();
            this.lblSkinInfo = new System.Windows.Forms.Label();
            this.btnDeleteSkinConfig = new System.Windows.Forms.Button();
            this.btnSaveSkinConfig = new System.Windows.Forms.Button();
            this.cboSkinConfig = new System.Windows.Forms.ComboBox();
            this.lblCharSelect = new System.Windows.Forms.Label();
            this.btnResetSkins = new System.Windows.Forms.Button();
            this.lvwSkins = new System.Windows.Forms.ListView();
            this.lvwCharacters = new System.Windows.Forms.ListView();
            this.lblSkinSelect = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chkSkinConfigAutosave = new System.Windows.Forms.CheckBox();
            this.chkModConfigAutosave = new System.Windows.Forms.CheckBox();
            this.chkModsReplaceSkins = new System.Windows.Forms.CheckBox();
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
            this.lblSkinVersion = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picModImage)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSkin)).BeginInit();
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
            this.tabControl1.Location = new System.Drawing.Point(2, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(672, 457);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblModName);
            this.tabPage1.Controls.Add(this.picModImage);
            this.tabPage1.Controls.Add(this.btnDeleteModConfig);
            this.tabPage1.Controls.Add(this.btnSaveModConfig);
            this.tabPage1.Controls.Add(this.cboModConfig);
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
            this.tabPage1.Size = new System.Drawing.Size(664, 431);
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
            // btnDeleteModConfig
            // 
            this.btnDeleteModConfig.Location = new System.Drawing.Point(288, 7);
            this.btnDeleteModConfig.Name = "btnDeleteModConfig";
            this.btnDeleteModConfig.Size = new System.Drawing.Size(49, 21);
            this.btnDeleteModConfig.TabIndex = 22;
            this.btnDeleteModConfig.Text = "Delete";
            this.toolTip1.SetToolTip(this.btnDeleteModConfig, "Deletes the currently selected mod configuration.");
            this.btnDeleteModConfig.UseVisualStyleBackColor = true;
            this.btnDeleteModConfig.Click += new System.EventHandler(this.btnDeleteModConfig_Click);
            // 
            // btnSaveModConfig
            // 
            this.btnSaveModConfig.Location = new System.Drawing.Point(238, 7);
            this.btnSaveModConfig.Name = "btnSaveModConfig";
            this.btnSaveModConfig.Size = new System.Drawing.Size(44, 21);
            this.btnSaveModConfig.TabIndex = 21;
            this.btnSaveModConfig.Text = "Save";
            this.toolTip1.SetToolTip(this.btnSaveModConfig, "Saves the currently selected mod configuration.");
            this.btnSaveModConfig.UseVisualStyleBackColor = true;
            this.btnSaveModConfig.Click += new System.EventHandler(this.btnSaveModConfig_Click);
            // 
            // cboModConfig
            // 
            this.cboModConfig.FormattingEnabled = true;
            this.cboModConfig.Location = new System.Drawing.Point(8, 7);
            this.cboModConfig.Name = "cboModConfig";
            this.cboModConfig.Size = new System.Drawing.Size(224, 21);
            this.cboModConfig.TabIndex = 19;
            this.cboModConfig.Text = "Default";
            this.cboModConfig.SelectedIndexChanged += new System.EventHandler(this.cboModConfig_SelectedIndexChanged);
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
            this.lblLoadOrder.Location = new System.Drawing.Point(6, 412);
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
            this.txtModInfo.Size = new System.Drawing.Size(256, 214);
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
            this.chklstMods.Size = new System.Drawing.Size(329, 364);
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
            this.tabPage2.Controls.Add(this.lblSkinVersion);
            this.tabPage2.Controls.Add(this.lblSkinFiller);
            this.tabPage2.Controls.Add(this.lnkSkinEmail);
            this.tabPage2.Controls.Add(this.lnkSkinWebUrl);
            this.tabPage2.Controls.Add(this.lblSkinList);
            this.tabPage2.Controls.Add(this.txtSkinInfo);
            this.tabPage2.Controls.Add(this.lblSkinText);
            this.tabPage2.Controls.Add(this.btnRestoreSkin);
            this.tabPage2.Controls.Add(this.picCharacter);
            this.tabPage2.Controls.Add(this.picSkin);
            this.tabPage2.Controls.Add(this.lblCharacter);
            this.tabPage2.Controls.Add(this.lblSkinInfo);
            this.tabPage2.Controls.Add(this.btnDeleteSkinConfig);
            this.tabPage2.Controls.Add(this.btnSaveSkinConfig);
            this.tabPage2.Controls.Add(this.cboSkinConfig);
            this.tabPage2.Controls.Add(this.lblCharSelect);
            this.tabPage2.Controls.Add(this.btnResetSkins);
            this.tabPage2.Controls.Add(this.lvwSkins);
            this.tabPage2.Controls.Add(this.lvwCharacters);
            this.tabPage2.Controls.Add(this.lblSkinSelect);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(664, 431);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Characters";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblSkinFiller
            // 
            this.lblSkinFiller.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSkinFiller.Location = new System.Drawing.Point(521, 190);
            this.lblSkinFiller.Margin = new System.Windows.Forms.Padding(0, 0, 3, 0);
            this.lblSkinFiller.Name = "lblSkinFiller";
            this.lblSkinFiller.Size = new System.Drawing.Size(138, 27);
            this.lblSkinFiller.TabIndex = 30;
            // 
            // lnkSkinEmail
            // 
            this.lnkSkinEmail.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lnkSkinEmail.Location = new System.Drawing.Point(477, 190);
            this.lnkSkinEmail.Margin = new System.Windows.Forms.Padding(0);
            this.lnkSkinEmail.Name = "lnkSkinEmail";
            this.lnkSkinEmail.Size = new System.Drawing.Size(44, 27);
            this.lnkSkinEmail.TabIndex = 29;
            this.lnkSkinEmail.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkSkinEmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSkinEmail_LinkClicked);
            // 
            // lnkSkinWebUrl
            // 
            this.lnkSkinWebUrl.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lnkSkinWebUrl.Location = new System.Drawing.Point(436, 190);
            this.lnkSkinWebUrl.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lnkSkinWebUrl.Name = "lnkSkinWebUrl";
            this.lnkSkinWebUrl.Size = new System.Drawing.Size(41, 27);
            this.lnkSkinWebUrl.TabIndex = 28;
            this.lnkSkinWebUrl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lnkSkinWebUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSkinWebUrl_LinkClicked);
            // 
            // lblSkinList
            // 
            this.lblSkinList.AutoSize = true;
            this.lblSkinList.Location = new System.Drawing.Point(1, 10);
            this.lblSkinList.Name = "lblSkinList";
            this.lblSkinList.Size = new System.Drawing.Size(56, 13);
            this.lblSkinList.TabIndex = 27;
            this.lblSkinList.Text = "Saved list:";
            // 
            // txtSkinInfo
            // 
            this.txtSkinInfo.Location = new System.Drawing.Point(436, 223);
            this.txtSkinInfo.Margin = new System.Windows.Forms.Padding(6);
            this.txtSkinInfo.Multiline = true;
            this.txtSkinInfo.Name = "txtSkinInfo";
            this.txtSkinInfo.ReadOnly = true;
            this.txtSkinInfo.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSkinInfo.Size = new System.Drawing.Size(223, 202);
            this.txtSkinInfo.TabIndex = 26;
            // 
            // lblSkinText
            // 
            this.lblSkinText.AutoSize = true;
            this.lblSkinText.Location = new System.Drawing.Point(528, 7);
            this.lblSkinText.Name = "lblSkinText";
            this.lblSkinText.Size = new System.Drawing.Size(101, 13);
            this.lblSkinText.TabIndex = 25;
            this.lblSkinText.Text = "Selected Character:";
            // 
            // btnRestoreSkin
            // 
            this.btnRestoreSkin.Location = new System.Drawing.Point(436, 69);
            this.btnRestoreSkin.Name = "btnRestoreSkin";
            this.btnRestoreSkin.Size = new System.Drawing.Size(96, 34);
            this.btnRestoreSkin.TabIndex = 12;
            this.btnRestoreSkin.Text = "Reset character";
            this.toolTip1.SetToolTip(this.btnRestoreSkin, "This will give the selected character its default skin.");
            this.btnRestoreSkin.UseVisualStyleBackColor = true;
            this.btnRestoreSkin.Click += new System.EventHandler(this.btnRestoreSkin_Click);
            // 
            // picCharacter
            // 
            this.picCharacter.BackColor = System.Drawing.Color.WhiteSmoke;
            this.picCharacter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picCharacter.Location = new System.Drawing.Point(464, 23);
            this.picCharacter.Name = "picCharacter";
            this.picCharacter.Size = new System.Drawing.Size(40, 40);
            this.picCharacter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picCharacter.TabIndex = 5;
            this.picCharacter.TabStop = false;
            // 
            // picSkin
            // 
            this.picSkin.BackColor = System.Drawing.Color.White;
            this.picSkin.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picSkin.Location = new System.Drawing.Point(538, 23);
            this.picSkin.Name = "picSkin";
            this.picSkin.Size = new System.Drawing.Size(80, 80);
            this.picSkin.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.picSkin.TabIndex = 8;
            this.picSkin.TabStop = false;
            // 
            // lblCharacter
            // 
            this.lblCharacter.AutoSize = true;
            this.lblCharacter.Location = new System.Drawing.Point(462, 7);
            this.lblCharacter.Name = "lblCharacter";
            this.lblCharacter.Size = new System.Drawing.Size(45, 13);
            this.lblCharacter.TabIndex = 9;
            this.lblCharacter.Text = "Original:";
            // 
            // lblSkinInfo
            // 
            this.lblSkinInfo.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSkinInfo.Location = new System.Drawing.Point(436, 110);
            this.lblSkinInfo.Name = "lblSkinInfo";
            this.lblSkinInfo.Size = new System.Drawing.Size(223, 58);
            this.lblSkinInfo.TabIndex = 24;
            // 
            // btnDeleteSkinConfig
            // 
            this.btnDeleteSkinConfig.Location = new System.Drawing.Point(380, 7);
            this.btnDeleteSkinConfig.Name = "btnDeleteSkinConfig";
            this.btnDeleteSkinConfig.Size = new System.Drawing.Size(50, 21);
            this.btnDeleteSkinConfig.TabIndex = 23;
            this.btnDeleteSkinConfig.Text = "Delete";
            this.toolTip1.SetToolTip(this.btnDeleteSkinConfig, "Deletes the currently selected skin configuration.");
            this.btnDeleteSkinConfig.UseVisualStyleBackColor = true;
            this.btnDeleteSkinConfig.Click += new System.EventHandler(this.btnDeleteSkinConfig_Click);
            // 
            // btnSaveSkinConfig
            // 
            this.btnSaveSkinConfig.Location = new System.Drawing.Point(330, 7);
            this.btnSaveSkinConfig.Name = "btnSaveSkinConfig";
            this.btnSaveSkinConfig.Size = new System.Drawing.Size(44, 21);
            this.btnSaveSkinConfig.TabIndex = 22;
            this.btnSaveSkinConfig.Text = "Save";
            this.toolTip1.SetToolTip(this.btnSaveSkinConfig, "Saves the currently selected skin configuration.");
            this.btnSaveSkinConfig.UseVisualStyleBackColor = true;
            this.btnSaveSkinConfig.Click += new System.EventHandler(this.btnSaveSkinConfig_Click);
            // 
            // cboSkinConfig
            // 
            this.cboSkinConfig.FormattingEnabled = true;
            this.cboSkinConfig.Location = new System.Drawing.Point(57, 7);
            this.cboSkinConfig.Name = "cboSkinConfig";
            this.cboSkinConfig.Size = new System.Drawing.Size(267, 21);
            this.cboSkinConfig.TabIndex = 14;
            this.cboSkinConfig.Text = "Default";
            this.cboSkinConfig.SelectedIndexChanged += new System.EventHandler(this.cboSkinConfig_SelectedIndexChanged);
            // 
            // lblCharSelect
            // 
            this.lblCharSelect.AutoSize = true;
            this.lblCharSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCharSelect.Location = new System.Drawing.Point(6, 405);
            this.lblCharSelect.Name = "lblCharSelect";
            this.lblCharSelect.Size = new System.Drawing.Size(223, 13);
            this.lblCharSelect.TabIndex = 10;
            this.lblCharSelect.Text = "Double-click a character to change it.";
            // 
            // btnResetSkins
            // 
            this.btnResetSkins.Location = new System.Drawing.Point(244, 398);
            this.btnResetSkins.Name = "btnResetSkins";
            this.btnResetSkins.Size = new System.Drawing.Size(186, 27);
            this.btnResetSkins.TabIndex = 6;
            this.btnResetSkins.Text = "Reset ALL characters";
            this.toolTip1.SetToolTip(this.btnResetSkins, "This will give all characters their default skins.");
            this.btnResetSkins.UseVisualStyleBackColor = true;
            this.btnResetSkins.Click += new System.EventHandler(this.btnResetSkins_Click);
            // 
            // lvwSkins
            // 
            this.lvwSkins.BackColor = System.Drawing.Color.AliceBlue;
            this.lvwSkins.HideSelection = false;
            this.lvwSkins.Location = new System.Drawing.Point(4, 34);
            this.lvwSkins.MultiSelect = false;
            this.lvwSkins.Name = "lvwSkins";
            this.lvwSkins.Size = new System.Drawing.Size(426, 358);
            this.lvwSkins.TabIndex = 7;
            this.lvwSkins.TileSize = new System.Drawing.Size(80, 80);
            this.lvwSkins.UseCompatibleStateImageBehavior = false;
            this.lvwSkins.Visible = false;
            this.lvwSkins.SelectedIndexChanged += new System.EventHandler(this.lvwSkins_SelectedIndexChanged);
            this.lvwSkins.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwSkins_MouseDoubleClick);
            // 
            // lvwCharacters
            // 
            this.lvwCharacters.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvwCharacters.HideSelection = false;
            this.lvwCharacters.Location = new System.Drawing.Point(4, 34);
            this.lvwCharacters.MultiSelect = false;
            this.lvwCharacters.Name = "lvwCharacters";
            this.lvwCharacters.Size = new System.Drawing.Size(426, 358);
            this.lvwCharacters.TabIndex = 4;
            this.lvwCharacters.TileSize = new System.Drawing.Size(80, 80);
            this.lvwCharacters.UseCompatibleStateImageBehavior = false;
            this.lvwCharacters.SelectedIndexChanged += new System.EventHandler(this.lvwCharacters_SelectedIndexChanged);
            this.lvwCharacters.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lvwCharacters_MouseDoubleClick);
            // 
            // lblSkinSelect
            // 
            this.lblSkinSelect.AutoSize = true;
            this.lblSkinSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSkinSelect.Location = new System.Drawing.Point(6, 405);
            this.lblSkinSelect.Name = "lblSkinSelect";
            this.lblSkinSelect.Size = new System.Drawing.Size(222, 13);
            this.lblSkinSelect.TabIndex = 11;
            this.lblSkinSelect.Text = "Double-click a character to choose it.";
            this.lblSkinSelect.Visible = false;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chkSkinConfigAutosave);
            this.tabPage3.Controls.Add(this.chkModConfigAutosave);
            this.tabPage3.Controls.Add(this.chkModsReplaceSkins);
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
            this.tabPage3.Size = new System.Drawing.Size(664, 431);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Settings";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chkSkinConfigAutosave
            // 
            this.chkSkinConfigAutosave.AutoSize = true;
            this.chkSkinConfigAutosave.Location = new System.Drawing.Point(13, 187);
            this.chkSkinConfigAutosave.Name = "chkSkinConfigAutosave";
            this.chkSkinConfigAutosave.Size = new System.Drawing.Size(157, 17);
            this.chkSkinConfigAutosave.TabIndex = 13;
            this.chkSkinConfigAutosave.Text = "Autosave skin configuration";
            this.toolTip1.SetToolTip(this.chkSkinConfigAutosave, "Enables saving the current skin configuration when patching or closing patchlunky" +
        ".");
            this.chkSkinConfigAutosave.UseVisualStyleBackColor = true;
            this.chkSkinConfigAutosave.CheckedChanged += new System.EventHandler(this.chkSkinConfigAutosave_CheckedChanged);
            // 
            // chkModConfigAutosave
            // 
            this.chkModConfigAutosave.AutoSize = true;
            this.chkModConfigAutosave.Location = new System.Drawing.Point(13, 164);
            this.chkModConfigAutosave.Name = "chkModConfigAutosave";
            this.chkModConfigAutosave.Size = new System.Drawing.Size(158, 17);
            this.chkModConfigAutosave.TabIndex = 12;
            this.chkModConfigAutosave.Text = "Autosave mod configuration";
            this.toolTip1.SetToolTip(this.chkModConfigAutosave, "Enables saving the current mod configuration when patching or closing patchlunky." +
        "");
            this.chkModConfigAutosave.UseVisualStyleBackColor = true;
            this.chkModConfigAutosave.CheckedChanged += new System.EventHandler(this.chkModConfigAutosave_CheckedChanged);
            // 
            // chkModsReplaceSkins
            // 
            this.chkModsReplaceSkins.AutoSize = true;
            this.chkModsReplaceSkins.Location = new System.Drawing.Point(13, 134);
            this.chkModsReplaceSkins.Name = "chkModsReplaceSkins";
            this.chkModsReplaceSkins.Size = new System.Drawing.Size(173, 17);
            this.chkModsReplaceSkins.TabIndex = 11;
            this.chkModsReplaceSkins.Text = "Mods can replace default skins";
            this.toolTip1.SetToolTip(this.chkModsReplaceSkins, "This setting allows mods to replace any character skins that have no custom skins" +
        " assigned to them.");
            this.chkModsReplaceSkins.UseVisualStyleBackColor = true;
            this.chkModsReplaceSkins.CheckedChanged += new System.EventHandler(this.chkModsReplaceSkins_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Miriam", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(112, 242);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(285, 32);
            this.label2.TabIndex = 10;
            this.label2.Text = "Under construction.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Patchlunky.Properties.Resources.under_construction;
            this.pictureBox1.Location = new System.Drawing.Point(27, 229);
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
            this.toolTip1.SetToolTip(this.btnRestore, "This will replace the game files with the backups that were created with the Patc" +
        "hlunky setup wizard.");
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
            this.toolTip1.SetToolTip(this.btnSetup, "You can press this button to force the Patchlunky Setup wizard to run again.");
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
            this.toolTip1.SetToolTip(this.btnGamePath, "Manually set the Spelunky path by browsing to the directory.");
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
            this.prgPatchGame.Location = new System.Drawing.Point(92, 463);
            this.prgPatchGame.Name = "prgPatchGame";
            this.prgPatchGame.Size = new System.Drawing.Size(476, 23);
            this.prgPatchGame.TabIndex = 23;
            // 
            // btnPatch
            // 
            this.btnPatch.Location = new System.Drawing.Point(3, 459);
            this.btnPatch.Name = "btnPatch";
            this.btnPatch.Size = new System.Drawing.Size(83, 32);
            this.btnPatch.TabIndex = 8;
            this.btnPatch.Text = "Patch Game";
            this.toolTip1.SetToolTip(this.btnPatch, "Patch the currently checked Mods and Skins to the game.\r\n(Make sure that you don\'" +
        "t have Spelunky opened when pressing this)");
            this.btnPatch.UseVisualStyleBackColor = true;
            this.btnPatch.Click += new System.EventHandler(this.btnPatch_Click);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(3, 497);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(667, 110);
            this.txtLog.TabIndex = 4;
            this.txtLog.WordWrap = false;
            // 
            // btnRunGame
            // 
            this.btnRunGame.Location = new System.Drawing.Point(574, 459);
            this.btnRunGame.Name = "btnRunGame";
            this.btnRunGame.Size = new System.Drawing.Size(96, 32);
            this.btnRunGame.TabIndex = 24;
            this.btnRunGame.Text = "Launch Game";
            this.toolTip1.SetToolTip(this.btnRunGame, "Launch Spelunky HD.");
            this.btnRunGame.UseVisualStyleBackColor = true;
            this.btnRunGame.Click += new System.EventHandler(this.btnRunGame_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 8000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // lblSkinVersion
            // 
            this.lblSkinVersion.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblSkinVersion.Location = new System.Drawing.Point(436, 168);
            this.lblSkinVersion.Name = "lblSkinVersion";
            this.lblSkinVersion.Size = new System.Drawing.Size(223, 22);
            this.lblSkinVersion.TabIndex = 31;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(674, 611);
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
            ((System.ComponentModel.ISupportInitialize)(this.picCharacter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picSkin)).EndInit();
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
        private System.Windows.Forms.Button btnDeleteModConfig;
        private System.Windows.Forms.Button btnSaveModConfig;
        private System.Windows.Forms.ComboBox cboModConfig;
        private System.Windows.Forms.Label lblModText;
        private System.Windows.Forms.Label lblModList;
        private System.Windows.Forms.Label lblLoadOrder;
        private System.Windows.Forms.ProgressBar prgPatchGame;
        private System.Windows.Forms.Button btnRunGame;
        private System.Windows.Forms.PictureBox picModImage;
        private System.Windows.Forms.Label lblModName;
        private System.Windows.Forms.ListView lvwCharacters;
        private System.Windows.Forms.Button btnResetSkins;
        private System.Windows.Forms.ListView lvwSkins;
        private System.Windows.Forms.PictureBox picSkin;
        private System.Windows.Forms.Label lblCharacter;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblSkinSelect;
        private System.Windows.Forms.Label lblCharSelect;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkModsReplaceSkins;
        private System.Windows.Forms.CheckBox chkModConfigAutosave;
        private System.Windows.Forms.ComboBox cboSkinConfig;
        private System.Windows.Forms.Button btnDeleteSkinConfig;
        private System.Windows.Forms.Button btnSaveSkinConfig;
        private System.Windows.Forms.CheckBox chkSkinConfigAutosave;
        private System.Windows.Forms.Label lblSkinInfo;
        private System.Windows.Forms.Button btnRestoreSkin;
        private System.Windows.Forms.PictureBox picCharacter;
        private System.Windows.Forms.Label lblSkinText;
        private System.Windows.Forms.TextBox txtSkinInfo;
        private System.Windows.Forms.Label lblSkinList;
        private System.Windows.Forms.LinkLabel lnkSkinWebUrl;
        private System.Windows.Forms.LinkLabel lnkSkinEmail;
        private System.Windows.Forms.Label lblSkinFiller;
        private System.Windows.Forms.Label lblSkinVersion;
    }
}

