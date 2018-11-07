<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.tvConstruct = New System.Windows.Forms.TreeView()
        Me.cms_Room = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.刷新ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.gpbDevices = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.cms_Room.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "icon1.png")
        Me.ImageList1.Images.SetKeyName(1, "icon17.png")
        Me.ImageList1.Images.SetKeyName(2, "icon7.png")
        Me.ImageList1.Images.SetKeyName(3, "mo1.png")
        '
        'tvConstruct
        '
        Me.tvConstruct.Location = New System.Drawing.Point(12, 22)
        Me.tvConstruct.Name = "tvConstruct"
        Me.tvConstruct.Size = New System.Drawing.Size(283, 391)
        Me.tvConstruct.TabIndex = 1
        '
        'cms_Room
        '
        Me.cms_Room.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.刷新ToolStripMenuItem})
        Me.cms_Room.Name = "cms_Room"
        Me.cms_Room.Size = New System.Drawing.Size(101, 26)
        Me.cms_Room.Text = "刷新"
        '
        '刷新ToolStripMenuItem
        '
        Me.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem"
        Me.刷新ToolStripMenuItem.Size = New System.Drawing.Size(100, 22)
        Me.刷新ToolStripMenuItem.Text = "刷新"
        '
        'gpbDevices
        '
        Me.gpbDevices.Location = New System.Drawing.Point(329, 4)
        Me.gpbDevices.Name = "gpbDevices"
        Me.gpbDevices.Size = New System.Drawing.Size(674, 427)
        Me.gpbDevices.TabIndex = 3
        Me.gpbDevices.TabStop = False
        Me.gpbDevices.Text = "设备"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.tvConstruct)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 4)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(304, 427)
        Me.GroupBox2.TabIndex = 4
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "楼层 房间"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1015, 442)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.gpbDevices)
        Me.Font = New System.Drawing.Font("Microsoft YaHei", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.MaximizeBox = False
        Me.Name = "MainForm"
        Me.Text = "Kincony"
        Me.cms_Room.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents tvConstruct As TreeView
    Friend WithEvents cms_Room As ContextMenuStrip
    Friend WithEvents 刷新ToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents gpbDevices As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
End Class
