﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
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
        Me.lvDevice = New System.Windows.Forms.ListView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.tvConstruct = New System.Windows.Forms.TreeView()
        Me.SuspendLayout()
        '
        'lvDevice
        '
        Me.lvDevice.LargeImageList = Me.ImageList1
        Me.lvDevice.Location = New System.Drawing.Point(289, 17)
        Me.lvDevice.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.lvDevice.Name = "lvDevice"
        Me.lvDevice.Size = New System.Drawing.Size(549, 406)
        Me.lvDevice.TabIndex = 0
        Me.lvDevice.UseCompatibleStateImageBehavior = False
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
        Me.tvConstruct.Location = New System.Drawing.Point(12, 17)
        Me.tvConstruct.Name = "tvConstruct"
        Me.tvConstruct.Size = New System.Drawing.Size(271, 406)
        Me.tvConstruct.TabIndex = 1
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(850, 441)
        Me.Controls.Add(Me.tvConstruct)
        Me.Controls.Add(Me.lvDevice)
        Me.Font = New System.Drawing.Font("Microsoft YaHei", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(134, Byte))
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "MainForm"
        Me.Text = "Kincony"
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents lvDevice As ListView
    Friend WithEvents ImageList1 As ImageList
    Friend WithEvents tvConstruct As TreeView
End Class
