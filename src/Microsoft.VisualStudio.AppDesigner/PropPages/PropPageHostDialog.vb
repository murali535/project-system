' Copyright (c) Microsoft.  All Rights Reserved.  Licensed under the Apache License, Version 2.0.  See License.txt in the project root for license information.

Imports Microsoft.VisualStudio.Editors.AppDesDesignerFramework
Imports System.Windows.Forms

Namespace Microsoft.VisualStudio.Editors.PropertyPages

    Public NotInheritable Class PropPageHostDialog
        Inherits BaseDialog
        'Inherits Form

        Private _propPage As PropPageUserControlBase
        Public WithEvents Cancel As Button
        Public WithEvents OK As Button
        Public WithEvents okCancelTableLayoutPanel As TableLayoutPanel
        Public WithEvents overArchingTableLayoutPanel As TableLayoutPanel
        Private _firstFocusHandled As Boolean

        ''' <summary>
        ''' Gets the F1 keyword to push into the user context for this property page
        ''' </summary>
        ''' <value></value>
        ''' <remarks></remarks>
        Protected Overrides Property F1Keyword() As String
            Get
                Dim keyword As String = MyBase.F1Keyword
                If String.IsNullOrEmpty(keyword) AndAlso _propPage IsNot Nothing Then
                    Return DirectCast(_propPage, IPropertyPageInternal).GetHelpContextF1Keyword()
                End If
                Return keyword
            End Get
            Set(Value As String)
                MyBase.F1Keyword = Value
            End Set
        End Property

        Public Property PropPage() As PropPageUserControlBase
            Get
                Return _propPage
            End Get
            Set(Value As PropPageUserControlBase)
                Me.SuspendLayout()
                If _propPage IsNot Nothing Then
                    'Remove previous page if any
                    overArchingTableLayoutPanel.Controls.Remove(_propPage)
                End If
                _propPage = Value
                If _propPage IsNot Nothing Then
                    'm_propPage.SuspendLayout()
                    Me.BackColor = Value.BackColor
                    Me.MinimumSize = Drawing.Size.Empty
                    Me.AutoSize = True

                    If (_propPage.PageResizable) Then
                        Me.FormBorderStyle = FormBorderStyle.Sizable
                    Else
                        Me.FormBorderStyle = FormBorderStyle.FixedDialog
                    End If

                    _propPage.Margin = New Padding(0, 0, 0, 0)
                    _propPage.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
                        Or AnchorStyles.Left) _
                        Or AnchorStyles.Right), AnchorStyles)
                    _propPage.TabIndex = 0
                    'overArchingTableLayoutPanel.SuspendLayout()
                    overArchingTableLayoutPanel.Controls.Add(_propPage, 0, 0)
                    'overArchingTableLayoutPanel.ResumeLayout(False)

                    'm_propPage.ResumeLayout(False)
                End If
                Me.ResumeLayout(False)
                Me.PerformLayout()
                SetFocusToPage()
            End Set
        End Property

#Region " Windows Form Designer generated code "

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(disposing As Boolean)
            If disposing Then
                If Not (_components Is Nothing) Then
                    _components.Dispose()
                End If
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private _components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <DebuggerStepThrough()> Private Sub InitializeComponent()
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PropPageHostDialog))
            Me.OK = New Button
            Me.Cancel = New Button
            Me.okCancelTableLayoutPanel = New TableLayoutPanel
            Me.overArchingTableLayoutPanel = New TableLayoutPanel
            Me.okCancelTableLayoutPanel.SuspendLayout()
            Me.overArchingTableLayoutPanel.SuspendLayout()
            Me.SuspendLayout()
            '
            'OK
            '
            resources.ApplyResources(Me.OK, "OK")
            Me.OK.DialogResult = DialogResult.OK
            Me.OK.Margin = New Padding(0, 0, 3, 0)
            Me.OK.Name = "OK"
            '
            'Cancel
            '
            resources.ApplyResources(Me.Cancel, "Cancel")
            Me.Cancel.CausesValidation = False
            Me.Cancel.DialogResult = DialogResult.Cancel
            Me.Cancel.Margin = New Padding(3, 0, 0, 0)
            Me.Cancel.Name = "Cancel"
            '
            'okCancelTableLayoutPanel
            '
            resources.ApplyResources(Me.okCancelTableLayoutPanel, "okCancelTableLayoutPanel")
            Me.okCancelTableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0!))
            Me.okCancelTableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0!))
            Me.okCancelTableLayoutPanel.Controls.Add(Me.Cancel, 1, 0)
            Me.okCancelTableLayoutPanel.Controls.Add(Me.OK, 0, 0)
            Me.okCancelTableLayoutPanel.Margin = New Padding(0, 6, 0, 0)
            Me.okCancelTableLayoutPanel.Name = "okCancelTableLayoutPanel"
            Me.okCancelTableLayoutPanel.RowStyles.Add(New RowStyle)
            '
            'overArchingTableLayoutPanel
            '
            resources.ApplyResources(Me.overArchingTableLayoutPanel, "overArchingTableLayoutPanel")
            Me.overArchingTableLayoutPanel.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
            Me.overArchingTableLayoutPanel.Controls.Add(Me.okCancelTableLayoutPanel, 0, 1)
            Me.overArchingTableLayoutPanel.Name = "overArchingTableLayoutPanel"
            Me.overArchingTableLayoutPanel.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
            Me.overArchingTableLayoutPanel.RowStyles.Add(New RowStyle)
            '
            'PropPageHostDialog
            '
            resources.ApplyResources(Me, "$this")
            Me.Controls.Add(Me.overArchingTableLayoutPanel)
            Me.Padding = New Padding(12, 12, 12, 12)
            Me.FormBorderStyle = FormBorderStyle.FixedDialog
            Me.HelpButton = True
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "PropPageHostDialog"
            ' Do not scale, the proppage will handle it. If we set AutoScale here, the page will expand twice, and becomes way huge
            'Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.okCancelTableLayoutPanel.ResumeLayout(False)
            Me.okCancelTableLayoutPanel.PerformLayout()
            Me.overArchingTableLayoutPanel.ResumeLayout(False)
            Me.overArchingTableLayoutPanel.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

#End Region

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <param name="ServiceProvider"></param>
        ''' <remarks></remarks>
        Public Sub New(ServiceProvider As IServiceProvider, F1Keyword As String)
            MyBase.New(ServiceProvider)

            'This call is required by the Windows Form Designer.
            InitializeComponent()

            'Add any initialization after the InitializeComponent() call
            Me.F1Keyword = F1Keyword

            Me.AcceptButton = Me.OK
            Me.CancelButton = Me.Cancel
        End Sub

        Protected Overrides Sub OnShown(e As EventArgs)
            MyBase.OnShown(e)

            If Me.MinimumSize.IsEmpty Then
                Me.MinimumSize = Me.Size
                Me.AutoSize = False
            End If
        End Sub

        Private Sub Cancel_Click(sender As Object, e As EventArgs) Handles Cancel.Click
            PropPage.RestoreInitialValues()
            Me.Close()
        End Sub

        Private Sub OK_Click(sender As Object, e As EventArgs) Handles OK.Click
            'Save the changes if current values
            Try
                'No errors in the values, apply & close the dialog
                If PropPage.IsDirty Then
                    PropPage.Apply()
                End If
                Me.Close()
            Catch ex As ValidationException
                _propPage.ShowErrorMessage(ex)
                ex.RestoreFocus()
                Return
            Catch ex As SystemException
                _propPage.ShowErrorMessage(ex)
                Return
            Catch ex As Exception When AppDesCommon.ReportWithoutCrash(ex, NameOf(OK_Click), NameOf(PropPageHostDialog))
                _propPage.ShowErrorMessage(ex)
                Return
            End Try
        End Sub

        Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
            If e.CloseReason = CloseReason.None Then
                ' That happens when the user clicks the OK button, but validation failed
                ' That is how we block the user leave when something wrong.
                e.Cancel = True
            ElseIf Me.DialogResult <> DialogResult.OK Then
                ' If the user cancelled the edit, we should restore the initial values...
                PropPage.RestoreInitialValues()
            End If
        End Sub

        Public Sub SetFocusToPage()
            If Not _firstFocusHandled AndAlso _propPage IsNot Nothing Then
                _firstFocusHandled = True
                For i As Integer = 0 To _propPage.Controls.Count - 1
                    With _propPage.Controls.Item(i)
                        If .CanFocus() Then
                            .Focus()
                            Return
                        End If
                    End With
                Next i
            End If
        End Sub

        Private Sub PropPageHostDialog_HelpButtonClicked(sender As System.Object, e As System.ComponentModel.CancelEventArgs) Handles MyBase.HelpButtonClicked
            e.Cancel = True
            ShowHelp()
        End Sub
    End Class

End Namespace

