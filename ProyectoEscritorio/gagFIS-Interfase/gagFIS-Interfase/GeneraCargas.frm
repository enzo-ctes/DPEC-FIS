VERSION 5.00
Object = "{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.0#0"; "MSCOMCTL.OCX"
Object = "{D18BBD1F-82BB-4385-BED3-E9D31A3E361E}#1.0#0"; "KewlButtonz.ocx"
Begin VB.Form Form5 
   AutoRedraw      =   -1  'True
   BorderStyle     =   0  'None
   Caption         =   "Generar Cargas Colectoras"
   ClientHeight    =   8955
   ClientLeft      =   0
   ClientTop       =   0
   ClientWidth     =   10215
   ControlBox      =   0   'False
   LinkTopic       =   "Form5"
   MaxButton       =   0   'False
   MDIChild        =   -1  'True
   MinButton       =   0   'False
   Moveable        =   0   'False
   ScaleHeight     =   8955
   ScaleWidth      =   10215
   ShowInTaskbar   =   0   'False
   Begin VB.Frame fraRepro 
      Caption         =   "Resprocesar Una Ruta"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   -1  'True
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   6435
      Left            =   4380
      TabIndex        =   15
      Top             =   480
      Width           =   2835
      Begin VB.ComboBox cboLote 
         Enabled         =   0   'False
         Height          =   315
         Left            =   840
         TabIndex        =   25
         Text            =   "Todos"
         Top             =   5520
         Width           =   1635
      End
      Begin VB.TextBox txtRuta 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   285
         Left            =   840
         TabIndex        =   23
         Top             =   5160
         Width           =   1635
      End
      Begin VB.TextBox txKey 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   375
         IMEMode         =   3  'DISABLE
         Index           =   1
         Left            =   300
         PasswordChar    =   "#"
         TabIndex        =   20
         Text            =   " "
         Top             =   4620
         Width           =   2175
      End
      Begin VB.TextBox txKey 
         BeginProperty Font 
            Name            =   "MS Sans Serif"
            Size            =   12
            Charset         =   0
            Weight          =   400
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         Height          =   390
         Index           =   0
         Left            =   300
         TabIndex        =   19
         Text            =   " "
         Top             =   4020
         Width           =   2175
      End
      Begin KewlButtonz.KewlButtons cmdCancRepro 
         Height          =   345
         Left            =   1500
         TabIndex        =   18
         Top             =   5940
         Width           =   1065
         _ExtentX        =   1879
         _ExtentY        =   609
         BTYPE           =   3
         TX              =   "&Cancelar"
         ENAB            =   -1  'True
         BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         COLTYPE         =   2
         FOCUSR          =   -1  'True
         BCOL            =   12632319
         BCOLO           =   8421631
         FCOL            =   12582912
         FCOLO           =   12582912
         MCOL            =   16777215
         MPTR            =   1
         MICON           =   "GeneraCargas.frx":0000
         UMCOL           =   -1  'True
         SOFT            =   0   'False
         PICPOS          =   0
         NGREY           =   0   'False
         FX              =   0
         HAND            =   0   'False
         CHECK           =   0   'False
         VALUE           =   0   'False
      End
      Begin KewlButtonz.KewlButtons cmdAcepRepro 
         Height          =   345
         Left            =   240
         TabIndex        =   17
         Top             =   5940
         Width           =   1065
         _ExtentX        =   1879
         _ExtentY        =   609
         BTYPE           =   3
         TX              =   "&Aceptar"
         ENAB            =   -1  'True
         BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         COLTYPE         =   2
         FOCUSR          =   -1  'True
         BCOL            =   12648384
         BCOLO           =   8454016
         FCOL            =   12582912
         FCOLO           =   12582912
         MCOL            =   16777215
         MPTR            =   1
         MICON           =   "GeneraCargas.frx":001C
         UMCOL           =   -1  'True
         SOFT            =   0   'False
         PICPOS          =   0
         NGREY           =   0   'False
         FX              =   0
         HAND            =   0   'False
         CHECK           =   0   'False
         VALUE           =   0   'False
      End
      Begin VB.Label Label4 
         AutoSize        =   -1  'True
         Caption         =   "Lote"
         Height          =   195
         Left            =   360
         TabIndex        =   26
         Top             =   5580
         Width           =   315
      End
      Begin VB.Label Label3 
         AutoSize        =   -1  'True
         Caption         =   "Ruta"
         Height          =   195
         Left            =   360
         TabIndex        =   24
         Top             =   5220
         Width           =   345
      End
      Begin VB.Label Label2 
         AutoSize        =   -1  'True
         Caption         =   "Contraseña"
         Height          =   195
         Left            =   300
         TabIndex        =   22
         Top             =   4380
         Width           =   810
      End
      Begin VB.Label Label1 
         AutoSize        =   -1  'True
         Caption         =   "Usuario"
         Height          =   195
         Left            =   300
         TabIndex        =   21
         Top             =   3780
         Width           =   540
      End
      Begin VB.Label lbRepro 
         Caption         =   "Label1"
         Height          =   3375
         Left            =   120
         TabIndex        =   16
         Top             =   300
         Width           =   2535
      End
   End
   Begin VB.Frame fraCmnd 
      BorderStyle     =   0  'None
      Height          =   6375
      Left            =   7080
      TabIndex        =   5
      Top             =   480
      Width           =   2775
      Begin VB.Frame frIntPdas 
         Height          =   915
         Left            =   360
         TabIndex        =   6
         Top             =   600
         Width           =   1875
         Begin VB.OptionButton optIntPdas 
            Caption         =   "Solo Interior"
            Height          =   315
            Index           =   0
            Left            =   180
            TabIndex        =   8
            Top             =   180
            Value           =   -1  'True
            Width           =   1335
         End
         Begin VB.OptionButton optIntPdas 
            Caption         =   "Incluir Posadas"
            Height          =   375
            Index           =   1
            Left            =   180
            TabIndex        =   7
            Top             =   480
            Width           =   1395
         End
      End
      Begin KewlButtonz.KewlButtons cmdDetener 
         Height          =   495
         Left            =   360
         TabIndex        =   9
         Top             =   2880
         Width           =   1875
         _ExtentX        =   3307
         _ExtentY        =   873
         BTYPE           =   3
         TX              =   "&Detener"
         ENAB            =   0   'False
         BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         COLTYPE         =   2
         FOCUSR          =   -1  'True
         BCOL            =   12632319
         BCOLO           =   8421631
         FCOL            =   12582912
         FCOLO           =   12582912
         MCOL            =   16777215
         MPTR            =   1
         MICON           =   "GeneraCargas.frx":0038
         UMCOL           =   -1  'True
         SOFT            =   0   'False
         PICPOS          =   0
         NGREY           =   0   'False
         FX              =   0
         HAND            =   0   'False
         CHECK           =   0   'False
         VALUE           =   0   'False
      End
      Begin KewlButtonz.KewlButtons cmdDescSel 
         Height          =   345
         Index           =   1
         Left            =   360
         TabIndex        =   10
         Top             =   180
         Width           =   885
         _ExtentX        =   1561
         _ExtentY        =   609
         BTYPE           =   3
         TX              =   "&Todo"
         ENAB            =   -1  'True
         BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         COLTYPE         =   2
         FOCUSR          =   -1  'True
         BCOL            =   12648384
         BCOLO           =   8454016
         FCOL            =   12582912
         FCOLO           =   12582912
         MCOL            =   16777215
         MPTR            =   1
         MICON           =   "GeneraCargas.frx":0054
         UMCOL           =   -1  'True
         SOFT            =   0   'False
         PICPOS          =   0
         NGREY           =   0   'False
         FX              =   0
         HAND            =   0   'False
         CHECK           =   0   'False
         VALUE           =   0   'False
      End
      Begin KewlButtonz.KewlButtons cmdDescSel 
         Height          =   345
         Index           =   0
         Left            =   1350
         TabIndex        =   11
         Top             =   180
         Width           =   885
         _ExtentX        =   1561
         _ExtentY        =   609
         BTYPE           =   3
         TX              =   "&Nada"
         ENAB            =   -1  'True
         BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         COLTYPE         =   2
         FOCUSR          =   -1  'True
         BCOL            =   12632319
         BCOLO           =   8421631
         FCOL            =   12582912
         FCOLO           =   12582912
         MCOL            =   16777215
         MPTR            =   1
         MICON           =   "GeneraCargas.frx":0070
         UMCOL           =   -1  'True
         SOFT            =   0   'False
         PICPOS          =   0
         NGREY           =   0   'False
         FX              =   0
         HAND            =   0   'False
         CHECK           =   0   'False
         VALUE           =   0   'False
      End
      Begin KewlButtonz.KewlButtons cmdGenerar 
         Height          =   495
         Left            =   360
         TabIndex        =   12
         Top             =   2280
         Width           =   1875
         _ExtentX        =   3307
         _ExtentY        =   873
         BTYPE           =   3
         TX              =   "&Generar"
         ENAB            =   -1  'True
         BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         COLTYPE         =   2
         FOCUSR          =   -1  'True
         BCOL            =   12648384
         BCOLO           =   8454016
         FCOL            =   12582912
         FCOLO           =   12582912
         MCOL            =   16777215
         MPTR            =   1
         MICON           =   "GeneraCargas.frx":008C
         UMCOL           =   -1  'True
         SOFT            =   0   'False
         PICPOS          =   0
         NGREY           =   0   'False
         FX              =   0
         HAND            =   0   'False
         CHECK           =   0   'False
         VALUE           =   0   'False
      End
      Begin KewlButtonz.KewlButtons cmdBuscar 
         Height          =   495
         Left            =   360
         TabIndex        =   13
         Top             =   1680
         Width           =   1875
         _ExtentX        =   3307
         _ExtentY        =   873
         BTYPE           =   3
         TX              =   "&Buscar "
         ENAB            =   -1  'True
         BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         COLTYPE         =   2
         FOCUSR          =   -1  'True
         BCOL            =   12648384
         BCOLO           =   8454016
         FCOL            =   12582912
         FCOLO           =   12582912
         MCOL            =   16777215
         MPTR            =   1
         MICON           =   "GeneraCargas.frx":00A8
         UMCOL           =   -1  'True
         SOFT            =   0   'False
         PICPOS          =   0
         NGREY           =   0   'False
         FX              =   0
         HAND            =   0   'False
         CHECK           =   0   'False
         VALUE           =   0   'False
      End
      Begin KewlButtonz.KewlButtons cmdReprocesar 
         Height          =   375
         Left            =   360
         TabIndex        =   14
         Top             =   5940
         Width           =   1875
         _ExtentX        =   3307
         _ExtentY        =   661
         BTYPE           =   3
         TX              =   "&Reprocesar"
         ENAB            =   -1  'True
         BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
            Name            =   "MS Sans Serif"
            Size            =   9.75
            Charset         =   0
            Weight          =   700
            Underline       =   0   'False
            Italic          =   0   'False
            Strikethrough   =   0   'False
         EndProperty
         COLTYPE         =   2
         FOCUSR          =   -1  'True
         BCOL            =   12648447
         BCOLO           =   8454143
         FCOL            =   192
         FCOLO           =   128
         MCOL            =   16777215
         MPTR            =   1
         MICON           =   "GeneraCargas.frx":00C4
         UMCOL           =   -1  'True
         SOFT            =   0   'False
         PICPOS          =   0
         NGREY           =   0   'False
         FX              =   0
         HAND            =   0   'False
         CHECK           =   0   'False
         VALUE           =   0   'False
      End
   End
   Begin MSComctlLib.ListView Lvw 
      Height          =   3915
      Left            =   600
      TabIndex        =   4
      Top             =   960
      Width           =   6195
      _ExtentX        =   10927
      _ExtentY        =   6906
      View            =   3
      LabelEdit       =   1
      Sorted          =   -1  'True
      LabelWrap       =   -1  'True
      HideSelection   =   -1  'True
      _Version        =   393217
      ForeColor       =   -2147483640
      BackColor       =   -2147483643
      BorderStyle     =   1
      Appearance      =   1
      NumItems        =   6
      BeginProperty ColumnHeader(1) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         Text            =   "orden"
         Object.Width           =   1058
      EndProperty
      BeginProperty ColumnHeader(2) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   1
         Text            =   "Codigo"
         Object.Width           =   1058
      EndProperty
      BeginProperty ColumnHeader(3) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   2
         Text            =   "Texto"
         Object.Width           =   1235
      EndProperty
      BeginProperty ColumnHeader(4) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   3
         Text            =   "Importe"
         Object.Width           =   2117
      EndProperty
      BeginProperty ColumnHeader(5) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   4
         Text            =   "Tipo"
         Object.Width           =   706
      EndProperty
      BeginProperty ColumnHeader(6) {BDD1F052-858B-11D1-B16A-00C0F0283628} 
         SubItemIndex    =   5
         Text            =   "Descrip"
         Object.Width           =   3528
      EndProperty
   End
   Begin MSComctlLib.StatusBar stBar 
      Align           =   2  'Align Bottom
      Height          =   375
      Left            =   0
      TabIndex        =   3
      Top             =   8580
      Width           =   10215
      _ExtentX        =   18018
      _ExtentY        =   661
      Style           =   1
      _Version        =   393216
      BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628} 
         NumPanels       =   1
         BeginProperty Panel1 {8E3867AB-8586-11D1-B16A-00C0F0283628} 
         EndProperty
      EndProperty
   End
   Begin MSComctlLib.ImageList imgList1 
      Left            =   6660
      Top             =   0
      _ExtentX        =   1005
      _ExtentY        =   1005
      BackColor       =   -2147483643
      ImageWidth      =   20
      ImageHeight     =   20
      MaskColor       =   12632256
      _Version        =   393216
   End
   Begin VB.Frame fraCargas 
      Caption         =   "Cargas a Generar"
      BeginProperty Font 
         Name            =   "MS Sans Serif"
         Size            =   9.75
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   -1  'True
         Strikethrough   =   0   'False
      EndProperty
      ForeColor       =   &H00C00000&
      Height          =   8025
      Left            =   240
      TabIndex        =   1
      Top             =   240
      Width           =   6585
      Begin MSComctlLib.TreeView tvwCargas 
         Height          =   7395
         Left            =   180
         TabIndex        =   2
         Top             =   420
         Width           =   6735
         _ExtentX        =   11880
         _ExtentY        =   13044
         _Version        =   393217
         Indentation     =   706
         LabelEdit       =   1
         LineStyle       =   1
         Sorted          =   -1  'True
         Style           =   7
         ImageList       =   "imgList1"
         BorderStyle     =   1
         Appearance      =   1
      End
   End
   Begin KewlButtonz.KewlButtons cmdCerrar 
      Height          =   345
      Left            =   8700
      TabIndex        =   0
      Top             =   7800
      Width           =   1275
      _ExtentX        =   2249
      _ExtentY        =   609
      BTYPE           =   3
      TX              =   "&Cerrar"
      ENAB            =   -1  'True
      BeginProperty FONT {0BE35203-8F91-11CE-9DE3-00AA004BB851} 
         Name            =   "MS Sans Serif"
         Size            =   12
         Charset         =   0
         Weight          =   700
         Underline       =   0   'False
         Italic          =   -1  'True
         Strikethrough   =   0   'False
      EndProperty
      COLTYPE         =   2
      FOCUSR          =   -1  'True
      BCOL            =   12632319
      BCOLO           =   16761087
      FCOL            =   128
      FCOLO           =   192
      MCOL            =   16777215
      MPTR            =   1
      MICON           =   "GeneraCargas.frx":00E0
      UMCOL           =   -1  'True
      SOFT            =   0   'False
      PICPOS          =   0
      NGREY           =   0   'False
      FX              =   0
      HAND            =   0   'False
      CHECK           =   0   'False
      VALUE           =   0   'False
   End
End
Attribute VB_Name = "Form5"
Attribute VB_GlobalNameSpace = False
Attribute VB_Creatable = False
Attribute VB_PredeclaredId = True
Attribute VB_Exposed = False
Option Explicit

Private Const codDVNoImprimir As Integer = 7 'codigo DV para no imprimir

'constantes usadas en el tree view
Private Const txRuta As String = "Libreta "
Private Const txZona As String = "Zona "
Private Const txSupZo As String = "ZONA "
Private Const txEMSA As String = "EMSA"
''''''''

Private Enum constEstadoConex
  NoCargado_CECx = 0
  Cargado_CECx = 1
  Descargado_CECx = 2
  Procesado_CECx = 3
  Exportado_CECx = 4
End Enum


'''''''''''''''''''''''''''''''''''''
Private pCancelar As Boolean
Private sListaConceptos As String
Private Const sFormatoListaConceptos As String = "-000000"
Private sListaCategorias As String
Private iDesZon As Integer    'para incluir posadas o no

Private Type tipoInfoNodo
  CnxSel As Long
  CnxTot As Long
  Key As String
  Texto As String
  Ruta As Long
  ZonaLetra  As String
  ZonaNro As Long
  Lote As Long
  Desde As Long
  Hasta As Long
  ArchIns As String
  ArchCarga As String
End Type

'Private Type tipoDatosRegistro
'  CantXSector As Integer
'  Total As Integer
'  Longitud As Integer
'  aLeer As Integer
'End Type

Private infNdX() As tipoInfoNodo
Private bColapso As Boolean
Private bBloqueoTvw As Boolean
Private sCargasCreadas() As String      'se agrega los textos de los nodos cuya carga se creó
Private sCargasNOCreadas() As String    'se agrega texto de los nodos cuya carga no se pudo crear
Private TmpCptoSP() As tipoConceptosSP
'


Private Sub cmdBuscar_Click()

  cmdGenerar.Enabled = False
  cmdDetener.Enabled = True
  cmdCerrar.Enabled = False
  DoEvents
  If Not bBloqueoTvw Then BuscarDatosIportacion
  cmdGenerar.Enabled = True
  cmdCerrar.Enabled = True
  cmdDetener.Enabled = False
  
End Sub


Private Sub cmdCerrar_Click()
  Unload Me
  CerrarForms
End Sub

Private Sub cmdDescSel_Click(Index As Integer)
Dim sQue As String
Dim ndX As Node
  'selecciona o deselecciona todas las cargas
  sQue = IIf(Index = 1, "Todo", "Nada")
  Set ndX = tvwCargas.Nodes("EMSA")
  ndX.Image = sQue
  AplicarEstadoAHijos ndX
  ndX.Image = "Logo"
  ContarConexionesPorRuta

End Sub

Private Sub cmdDetener_Click()
  pCancelar = True
End Sub

Private Sub cmdGenerar_Click()
Dim ctrC As Control
Dim sc As String

Dim tiempo As Double

  tiempo = Time
  pCancelar = False
  'bloquear posibilidad de modificaciones
  bBloqueoTvw = True
  For Each ctrC In Form5.Controls
    sc = ctrC.Name
    If TypeOf ctrC Is KewlButtons Then
        ctrC.Enabled = False
    End If
  Next
  cmdDetener.Enabled = True
  
  'genera las cargas
  GenerarCargas
  cmdCerrar.Enabled = True
  
  tiempo = Time - tiempo
  stBar.SimpleText = stBar.SimpleText & " - Tiempo empleado = " & Format(tiempo, "hh:mm:ss")
End Sub


Private Sub cmdReprocesar_Click()
  
  fraRepro.Visible = True
  fraRepro.Top = fraCmnd.Top
  fraRepro.Left = fraCmnd.Left
  fraRepro.Width = fraCmnd.Width
  lbRepro.Width = fraRepro.Width - lbRepro.Left * 2
    
  txKey(0).Text = ""
  txKey(1).Text = ""
    
  lbRepro = "- Ingrese su Usuario y Contraseña." & vbCrLf & _
            " - Introduzca el número de ruta que desea reprocesar, " & _
            " y pulse en ''Aceptar''. " & vbCrLf & _
            " - Seleccione el Lote que desea reprocesar y vuelva a pulsar en ''Aceptar''." & vbCrLf & _
            " - Si desea modificar la forma en que se ''corta'' la ruta, efectue la modificación " & _
            "correspondiente en el archivo ini, antes se proseguir." & vbCrLf & _
            " - Luego pulse el botón ''Buscar'', y continúe normalmente" & vbCrLf & _
            " - La ruta indicada se mostrará como ''No Procesada''"
  
  txKey(0).SetFocus
  
  cboLote.Enabled = False
  txtRuta.Enabled = True
  
End Sub

Private Sub cmdCancRepro_Click()
  fraRepro.Visible = False
End Sub

Private Sub cmdAcepRepro_Click()

  If cboLote.Enabled Then
    ResetRutaLote
    fraRepro.Visible = False
  Else
    If Val(txtRuta.Text) > 0 Then
      BuscarLotesDeRuta
    Else
      MsgBox "  Ruta NO Válida  "
    End If
  End If

End Sub

Private Sub BuscarLotesDeRuta()
Dim dbClave As Database
Dim rsClave As Recordset
Dim sQ As String
Dim bPermitido As Boolean

  'verifica Usuario y Contraseña
  bPermitido = False
  Set dbClave = OpenDatabase(App.Path & "\Recursos\DatosIns0.mdb")
  
  sQ = " SELECT * FROM Varios " & _
      " WHERE Usuario = '" & Trim(txKey(0).Text) & "'"
  Set rsClave = dbClave.OpenRecordset(sQ, dbOpenDynaset)
  
  If rsClave.RecordCount > 0 Then
    sQ = rsClave.Fields("Clave").Value
    If Trim(txKey(1).Text) = Trim(rsClave.Fields("Clave").Value) Then
      bPermitido = True
    Else
      MsgBox "     La clave ingresada NO es válida     ", vbInformation + vbOKOnly, " Clave Errónea "
    End If
  Else
    MsgBox "      No existe el usuario:  " & Trim(txKey(1).Text), vbInformation + vbOKOnly
  End If
  
  If bPermitido Then
    txtRuta.Enabled = False
    cboLote.Enabled = True
    cboLote.Clear
    cboLote.AddItem "Todos"
    cboLote.Text = "Todos"
  
    sQ = " SELECT DISTINCT LoteCarga FROM DatosIns " & _
          " WHERE Ruta = " & CLng(txtRuta.Text) & _
          " AND (Estado = " & constEstadoConex.Cargado_CECx & _
          " OR Estado = " & constEstadoConex.NoCargado_CECx & ")"
    Set rsClave = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
    If rsClave.RecordCount = 0 Then
      MsgBox "    No hay Datos Reprocesables para la Ruta: " & txtRuta.Text & "   ", vbInformation
    Else
      rsClave.MoveFirst
      Do Until rsClave.EOF
        cboLote.AddItem rsClave.Fields("LoteCarga").Value
        rsClave.MoveNext
      Loop
    End If
  End If
  
  On Error Resume Next
  rsClave.Close
  dbClave.Close
  Set dbClave = Nothing
  Set rsClave = Nothing
  On Error GoTo 0

End Sub


Private Sub ResetRutaLote()
Dim sQ As String
Dim iNoLe As Integer, i As Integer, ib As Integer
Dim sLista() As String
Dim sCarp As String

'poner el estado de la conexion como no cargado
  On Error GoTo errorAca
  
  iNoLe = FtPhX.GetIni("Novedades2", "No Leido")
  
  sQ = " UPDATE DatosIns " & _
       " SET LoteCarga = 0, Estado = " & constEstadoConex.NoCargado_CECx & _
       " WHERE Ruta = " & CLng(txtRuta.Text) & _
       " AND Estado = " & constEstadoConex.Cargado_CECx & _
       " AND Novedad2 = " & iNoLe
  If Trim(UCase(cboLote.Text)) <> "TODOS" Then
    sQ = sQ & " AND LoteCarga = " & Trim(cboLote.Text)
  End If
  BaseDatIns.Execute (sQ)
      
'si hay un archivo de carga generado el mismo debe ser borrado
  sQ = FtPhX.GetIniSplit("Ruta-Localidad", Format(CLng(txtRuta.Text), "00000"), ";", "Localidad de Emision")
  FtPhX.SetParametro "Distrito", sQ
  FtPhX.SetParametro "Lote", Trim(cboLote.Text)
  FtPhX.SetParametro "Ruta", CLng(txtRuta.Text)
  sCarp = FtPhX.GetIni("Carpetas", "Dir Cargas Enviar")
  If Trim(UCase(cboLote.Text)) = "TODOS" Then
    sQ = FtPhX.GetIniSplit("Carpetas", "Fil Cargas Enviar", "\", "Periodo", "Ruta")
  Else
    sQ = FtPhX.GetIniSplit("Carpetas", "Fil Cargas Enviar", "\", "Periodo", "Ruta", "Lote")
  End If
  
  ReDim sLista(0)
  With gFilParam
    .sFileRoot = QualifyPath(sCarp)               'path donde inicia
    .sFileNameExt = sQ & "-datos.txt"             'tipos de archivos a buscar
    .bRecurse = True '"Poner_tipo_de_busqueda"    'True = busqueda recursiva"
    .nCount = 0                                   'encotrados
    .nSearched = 0                                'buscados
    .bFindOrExclude = 1 '"Mostrar_cumplen_o_no"      '1=muestra los que cumplen sFileNameExt                                                    '0=muestra los que NO cumplen sFileNameExt
  End With

  Call SearchForFiles(gFilParam.sFileRoot, sLista())
  
  ib = UBound(sLista, 1)
  For i = 0 To ib
    If fso.FileExists(sLista(i)) Then
      sQ = fso.GetFile(sLista(i)).Name
      sQ = Left(sQ, Len(sQ) - 4)
      CrearSiNoExiste sCarp & "\Tomadas"
      sQ = sCarp & "\Tomadas\" & sQ & "-Borrado" & Format(Now, "yyyymmdd-HHnn") & ".txt"
      fso.MoveFile sLista(i), sQ
      If fso.FileExists(sLista(i)) Then
        Err.Raise 20055, "Reset Ruta Lote", "No se pudo borrar el archivo " & vbCrLf & _
                                            sLista(i)
      End If
    End If
  Next
  
errorAca:
  If Err.Number <> 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
      
End Sub



Private Sub Form_Load()
  
  With imgList1
    .ListImages.Add 1, "Todo", LoadPicture(App.Path & "\Recursos\ICONOS\Regular\Todo.gif")
    .ListImages.Add 2, "Algo", LoadPicture(App.Path & "\Recursos\ICONOS\Regular\Algo.gif")
    .ListImages.Add 3, "Nada", LoadPicture(App.Path & "\Recursos\ICONOS\Regular\Nada.gif")
    .ListImages.Add 4, "GenOk", LoadPicture(App.Path & "\Recursos\ICONOS\regular\Symbol-Check-2.gif")
    .ListImages.Add 5, "Error", LoadPicture(App.Path & "\Recursos\ICONOS\regular\Symbol-Error-3.gif")
    .ListImages.Add 6, "EnPro", LoadPicture(App.Path & "\Recursos\ICONOS\regular\Favorites.gif")
    .ListImages.Add 7, "Logo", LoadPicture(App.Path & "\Recursos\ICONOS\regular\Logo EMSA.bmp")
  End With
  ReDim infNdX(0)
  bBloqueoTvw = False
  
End Sub

Private Sub Form_Resize()
  
  cmdCerrar.Top = Form5.ScaleHeight - cmdCerrar.Height - 100 - stBar.Height
  cmdCerrar.Left = Form5.ScaleWidth - cmdCerrar.Width - 200
  
  fraCargas.Top = 100
  fraCargas.Left = 100
  fraCargas.Width = Me.ScaleWidth - fraCmnd.Width - fraCargas.Left - 200
  fraCargas.Height = Me.ScaleHeight - stBar.Height - fraCargas.Top - 100
  
  tvwCargas.Top = 300
  tvwCargas.Left = 100
  tvwCargas.Width = fraCargas.Width - 250
  tvwCargas.Height = fraCargas.Height - tvwCargas.Top - 100
  
  Lvw.Top = fraCargas.Top + 100
  Lvw.Left = fraCargas.Left + 100
  
  fraCmnd.Top = fraCargas.Top
  fraCmnd.Left = (Me.Width - fraCargas.Left + fraCargas.Width - fraCmnd.Width) / 2 + 100
  
  fraCargas.ZOrder
  
End Sub


Private Sub BuscarDatosIportacion()
'busca en la carpeta del periodo de importacion todos los datos.ins
'que correspondan al interior, esto es zona >=10
Dim bOk As Boolean
  On Error GoTo errorAca
  
  CarpImportacion = FtPhX.GetIni("Carpetas", "Dir Importacion", "Periodo")
  
  pCancelar = False
  
  tvwCargas.Nodes.Clear
  Me.MousePointer = vbHourglass
  tvwCargas.Nodes.Add , tvwChild, txEMSA, txEMSA, "Logo"
  tvwCargas.Nodes(txEMSA).Expanded = True
  
  'buscar los datos ins y actualizar punteros si correspponde
  bOk = BuscarDatosIns
  If pCancelar Then Err.Raise 20015, " Buscar Datos Importación ", "Proceso Cancelado por el Usuario "
  'asignar nros de lote
  If bOk Then bOk = AsignarNrosLotes
  If pCancelar Then Err.Raise 20015, " Buscar Datos Importación ", "Proceso Cancelado por el Usuario "
  'agregar las rutas al tree view
  If bOk Then bOk = CargarListaDeRutas
  If pCancelar Then Err.Raise 20015, " Buscar Datos Importación ", "Proceso Cancelado por el Usuario "
  'activar las cantidades en el tree view
  If bOk Then bOk = ContarConexionesPorRuta
  If pCancelar Then Err.Raise 20015, " Buscar Datos Importación ", "Proceso Cancelado por el Usuario "
  
  Me.MousePointer = 0
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  
End Sub


Private Function BuscarDatosIns() As Boolean
'buscar los datos ins y actualizar punteros si correspponde
Dim fdCar As Folder
Dim sc As String, sQm As String
Dim lZ As Long
Dim i As Integer

  On Error GoTo errorAca
  
  BuscarDatosIns = False
  iDesZon = IIf(optIntPdas(0), 10, 0)
  
  If fso.FolderExists(CarpImportacion) Then
    For Each fdCar In fso.GetFolder(CarpImportacion).SubFolders
      'ver a que zona pertenece la carpeta
      sc = fdCar.Name
      For i = 1 To Len(sc)
        If Val(Mid(sc, i)) > iDesZon Then
          'es una zona del interior, ver si existe datos.ins
          lZ = Val(Mid(sc, i))
          If fso.FileExists(fdCar.Path & "\Datos.ins") Then
            With ArchIns
                .Directorio = fdCar.Path
                .RutNombre = fdCar.Path & "\Datos.ins"
                .Nro = FreeFile
                Open .RutNombre For Random Access Read As #.Nro Len = Len(RegIns)
                .Size = LOF(.Nro)
                .CantReg = .Size \ Len(RegIns)
                VerSiActualizaPuntero pCancelar, -Me.ScaleWidth, fraCargas.Top
                If pCancelar Then Exit For
                Close .Nro
              End With
            Exit For
          End If
        End If
      Next
      If pCancelar Then Err.Raise 20015, "Cargar Datos Ins ", " Proceso Cancelado por el Usuario "
    Next
  Else
    MsgBox "  No existe la carpeta:  " & vbCrLf & _
           "  " & CarpImportacion, vbInformation + vbOKOnly, " No Hay Datos Importados "
  End If
  BuscarDatosIns = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  
End Function


Private Function CargarListaDeRutas() As Boolean
'Dim fdCar As Folder
Dim sc As String, sQm As String
Dim lZ As Long
Dim i As Integer
Dim sDistr As String
'Dim ndX As Node
Dim bCancelar As Boolean
Dim rtR As Recordset, rtZ As Recordset
Dim sZ As String, sR As String, cZ As String
Dim iR As Integer, iCntCx As Integer, iL As Integer
Dim iNoLe As Integer

  On Error GoTo errorAca
  CargarListaDeRutas = False
  
  'agregar las rutas al tree view
  stBar.SimpleText = " Buscando las Libretas Disponibles "
  DoEvents
  iNoLe = FtPhX.GetIni("Novedades2", "No Leido")
  sQm = " SELECT DISTINCT Ruta,LoteCarga " & _
        " FROM DatosIns " & _
        " WHERE DatosIns.Zona > " & iDesZon & _
        " AND DatosIns.Estado = " & NoCargado_CECx & _
        " AND DatosIns.Novedad2 = " & iNoLe
  Set rtZ = BaseDatIns.OpenRecordset(sQm, dbOpenDynaset)
  If rtZ.RecordCount > 0 Then
    rtZ.MoveLast
    rtZ.MoveFirst
    Do Until rtZ.EOF
      iR = rtZ.Fields("Ruta").Value
      iL = rtZ.Fields("LoteCarga").Value
      ZonaDistritoDeRuta iR, sDistr, lZ, cZ
      sDistr = LetraCapital(sDistr)
      If AgregarNodoDistrito(lZ, sDistr) Then
        'si agrego o ya estaba el distrito, agrega la zona
        sZ = txZona & Format(lZ, "000")
        If AgregarNodoZona(sZ, sDistr, cZ) Then
          'si agrego la zona, agrega la ruta
          sR = txRuta & Trim(iR) & " - Lote " & Trim(iL)
          iCntCx = CantidadConexionesRuta(iR, iL)
          sc = sR & " - Conex: " & Trim(iCntCx)
          tvwCargas.Nodes.Add sZ, tvwChild, sR, sc, "Todo"
          i = tvwCargas.Nodes(sR).Index
          If i > UBound(infNdX, 1) Then ReDim Preserve infNdX(i)
          'buscar los datos del lote
          sQm = " SELECT * FROM Lotes " & _
                " WHERE Lote = " & iL
          Set rtR = BaseDatIns.OpenRecordset(sQm, dbOpenDynaset)
          If rtR.RecordCount > 0 Then
            rtR.MoveFirst
            With infNdX(i)
              .CnxSel = iCntCx
              .CnxTot = iCntCx
              .Key = sR
              .ZonaLetra = cZ
              .ZonaNro = lZ
              .Ruta = iR
              .Desde = rtR.Fields("Desde").Value
              .Hasta = rtR.Fields("Hasta").Value
              .Lote = iL
              .Texto = txRuta & Trim(iR) & "  Lote " & Trim(iL) & _
                       " - Connex: " & Trim(iCntCx) & _
                       " - (" & Format(.Desde, "0000-000") & _
                       " a " & Format(.Hasta, "0000-000") & ")"
              tvwCargas.Nodes(sR).Text = .Texto
            End With
          End If
          tvwCargas.Nodes(sR).Expanded = True
        End If
      End If
      rtZ.MoveNext
      DoEvents
      If pCancelar Then Err.Raise 20015, "Cargar Lista de Rutas ", " Proceso Cancelado por el Usuario "
    Loop
  End If
  
  stBar.SimpleText = " "
  
  CargarListaDeRutas = True
  DoEvents
  
  Set rtZ = Nothing
  Set rtR = Nothing
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
    
End Function


Private Function CantidadConexionesRuta(iRuta As Integer, iLote As Integer) As Integer
'devuelve la cantidad de conexiones no cargadas de la libreta
Dim sQ As String
Dim iC As Integer
Dim rTs As Recordset
Dim iNoLe As Integer
  
  iNoLe = FtPhX.GetIni("Novedades2", "No Leido")

  sQ = " SELECT COUNT(Conexion) AS Cantidad FROM DatosIns " & _
        " WHERE Ruta = " & iRuta & _
        " AND LoteCarga =  " & iLote & _
        " AND Estado = " & NoCargado_CECx & _
        " AND Novedad2 = " & iNoLe
  Set rTs = BaseDatIns.OpenRecordset(sQ)
  iC = rTs.Fields("Cantidad").Value
  CantidadConexionesRuta = iC
  Set rTs = Nothing

End Function


Private Function AgregarNodoDistrito(lZona As Long, sDistrito As String) As Boolean
  'verifica si existe el nodo del distito al que corresponde la zona,
  'si no está lo agrega
  Dim ndX As Node
  Dim sQ As String
  Dim i As Integer, j As Integer
     
    On Error GoTo errorAca
    AgregarNodoDistrito = False
    
    sQ = LetraCapital(sDistrito)
    
    If BuscarIndiceNodo(tvwCargas, sQ) >= 0 Then
    AgregarNodoDistrito = True
    Exit Function
    End If
    On Error GoTo errorAca
    
    'agregar el nodo
    tvwCargas.Nodes.Add txEMSA, tvwChild, sQ, sQ, "Todo"
    tvwCargas.Nodes(sQ).Expanded = True
    i = tvwCargas.Nodes(sQ).Index
    If i > UBound(infNdX, 1) Then ReDim Preserve infNdX(i)
    infNdX(i).Texto = sQ
    infNdX(i).Key = sQ
    
    If BuscarIndiceNodo(tvwCargas, sQ) >= 0 Then
    AgregarNodoDistrito = True
    End If
    
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  
End Function


Private Function AgregarNodoZona(sZona As String, sDistrito As String, sLetrZon As String) As Boolean
'verifica si existe el nodo de la zona, si no está lo agrega
'si ya estaba o pudo agregarlo, devuelve true
  Dim ndX As Node
  Dim sQ As String
  Dim i As Integer
    
    AgregarNodoZona = False
    If BuscarIndiceNodo(tvwCargas, sZona) >= 0 Then
      AgregarNodoZona = True
      Exit Function
    End If
    On Error GoTo errorAca
    
    If Not AgregarNodoSuperZona(sDistrito, sLetrZon) Then Exit Function

    'agregar el nodo
    tvwCargas.Nodes.Add sDistrito & "-" & sLetrZon, tvwChild, sZona, sZona, "Todo"
    tvwCargas.Nodes(sZona).Expanded = True
    i = tvwCargas.Nodes(sZona).Index
    If i > UBound(infNdX, 1) Then ReDim Preserve infNdX(i)
    infNdX(i).Texto = sZona
    infNdX(i).Key = sZona
    
    If BuscarIndiceNodo(tvwCargas, sZona) >= 0 Then
      AgregarNodoZona = True
    End If
       
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  
End Function


Private Function AgregarNodoSuperZona(sDistrito As String, sLetrZon As String) As Boolean
'verifica si existe el nodo de la zona, si no está lo agrega
'si ya estaba o pudo agregarlo, devuelve true
  Dim ndX As Node, ndHijo As Node
  Dim sQ As String
  Dim i As Integer
  Dim sClave As String
  
    sClave = sDistrito & "-" & sLetrZon
     
    AgregarNodoSuperZona = False
    If BuscarIndiceNodo(tvwCargas, sClave) >= 0 Then
      AgregarNodoSuperZona = True
      Exit Function
    End If

    'agregar el nodo
    tvwCargas.Nodes.Add sDistrito, tvwChild, sClave, txSupZo & sLetrZon, "Todo"
    tvwCargas.Nodes(sClave).Expanded = True
    i = tvwCargas.Nodes(sClave).Index
    If i > UBound(infNdX, 1) Then ReDim Preserve infNdX(i)
    infNdX(i).Texto = "ZONA " & sLetrZon
    infNdX(i).Key = sClave
  
    If BuscarIndiceNodo(tvwCargas, sClave) >= 0 Then
      AgregarNodoSuperZona = True
    End If
    
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  
End Function



Private Sub tvwCargas_Collapse(ByVal Node As MSComctlLib.Node)
Dim ndX As Node

'  Node.Image = Node.Tag
  If Node.Key = txEMSA Then
    Node.Expanded = True
    If Node.Children > 0 Then
      Set ndX = Node.Child.FirstSibling
      Do
        ndX.Expanded = False
        If ndX = ndX.LastSibling Then Exit Do
        Set ndX = ndX.Next
      Loop
    End If
  End If
  
End Sub


Private Sub tvwCargas_Expand(ByVal Node As MSComctlLib.Node)
'  Node.Image = Node.Tag
End Sub


Private Sub tvwCargas_NodeClick(ByVal Node As MSComctlLib.Node)
  Dim i As Integer
  
    If bBloqueoTvw Then Exit Sub
    Node.Tag = Node.Image
  
    i = Node.Index
    Select Case Node.Image
      Case "Todo"
        Node.Image = "Nada"
        If Node.Children = 0 Then infNdX(i).CnxSel = 0
      Case Else
        Node.Image = "Todo"
        If Node.Children = 0 Then infNdX(i).CnxSel = infNdX(i).CnxTot
    End Select
    
    AplicarEstadoAHijos Node
    ContarConexionesPorRuta
    
End Sub


Private Sub AplicarEstadoAHijos(ByVal ndX As Node)
'aplica el mismo estado del nodo a sus hijos
Dim ndH As Node

  If ndX.Children > 0 Then
    Set ndH = ndX.Child.FirstSibling
    Do
      If pCancelar Then Err.Raise 20015, "Aplicar Estado a Hijos ", " Proceso Cancelado por el Usuario "
      ndH.Image = ndX.Image
      AplicarEstadoAHijos ndH
      If ndX.Image = "Todo" Then infNdX(ndH.Index).CnxSel = infNdX(ndH.Index).CnxTot
      If ndX.Image = "Nada" Then infNdX(ndH.Index).CnxSel = 0
      If ndH = ndH.LastSibling Then Exit Do
      Set ndH = ndH.Next
    Loop
  End If

End Sub


Private Sub AplicarEstadoAPadres(ByVal ndX As Node)
'suma las conexiones seleccionadas de los hijos y las aplica a los padres
'si todas están seleccionadas pone la Imagen "Todo"
'si ninguna está seleccionada pone la inagen "Nada"
'si al alguna seleccionadas pero no todas ppone la imagen "Algo"
Dim ndP As Node
Dim ndH As Node
Dim iP As Integer
Dim iH As Integer

  If ndX = ndX.Root Then Exit Sub
  Set ndP = ndX.Parent
  iP = ndP.Index
  infNdX(iP).CnxSel = 0
  infNdX(iP).CnxTot = 0
  
  Set ndH = ndX.FirstSibling
  Do
    iH = ndH.Index
    infNdX(iP).CnxSel = infNdX(iP).CnxSel + infNdX(iH).CnxSel
    infNdX(iP).CnxTot = infNdX(iP).CnxTot + infNdX(iH).CnxTot
    If ndH.Index = ndH.LastSibling.Index Then Exit Do
    Set ndH = ndH.Next
  Loop

  Select Case infNdX(iP).CnxSel
    Case 0
      ndP.Image = "Nada"
    Case infNdX(iP).CnxTot
      ndP.Image = "Todo"
    Case Else
      ndP.Image = "Algo"
  End Select
  
  ndP.Text = infNdX(iP).Texto & " - Conex: " & Trim(infNdX(iP).CnxSel) & " de " & Trim(infNdX(iP).CnxTot)
  If pCancelar Then Err.Raise 20015, "Generar Cargas ", " Proceso Cancelado por el Usuario "
  If ndP <> ndX.Root Then
    AplicarEstadoAPadres ndP
  End If
  
  tvwCargas.Nodes(txEMSA).Image = "Logo"
  'tvwCargas.Nodes(txEMSA).Text = "EMSA " ' -  Conex: " & Trim(infNdX(0).CnxSel) & " de " & Trim(infNdX(0).CnxTot)
  
End Sub


Private Function ContarConexionesPorRuta() As Boolean
'en cada nodo se traslada al ultimo y traslada el estado hacia arriba
Dim ndH As Node, ndP As Node
    
    On Error GoTo errorAca
    ContarConexionesPorRuta = False
    For Each ndP In tvwCargas.Nodes
      If ndP.Children = 0 Then
        AplicarEstadoAPadres ndP
      End If
    Next
    ContarConexionesPorRuta = True

errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
End Function

Private Function BuscarIndiceNodo(tvwDondeBuscar As TreeView, sClave As String) As Integer
Dim i As Integer
  'si no encuentra devuelve -1
    On Error Resume Next
    i = -1
    i = tvwDondeBuscar.Nodes(sClave).Index
    Err = 0
    BuscarIndiceNodo = i
    
End Function


Private Function AsignarNrosLotes() As Boolean
Dim sQm As String
Dim iR As Integer
Dim rtZ As Recordset
'Asigna un Nro de lote a todas las rutas en condiciones de ser cargadas

  AsignarNrosLotes = False
  On Error GoTo errorAca
  
  stBar.SimpleText = " Asignando Números de Lotes "
  sQm = BaseDatIns.Name
  sQm = " SELECT DISTINCT Ruta FROM DatosIns " & _
        " WHERE Zona > " & iDesZon & " AND LoteCarga = 0 AND Estado = " & NoCargado_CECx
  Set rtZ = BaseDatIns.OpenRecordset(sQm, dbOpenDynaset)
  If rtZ.RecordCount > 0 Then
    rtZ.MoveLast
    rtZ.MoveFirst
    Do Until rtZ.EOF
      iR = rtZ.Fields("Ruta").Value
      stBar.SimpleText = " Asignando Números de Lotes a Libreta " & Trim(iR)
      EstablecerLotes CLng(iR), 0
      rtZ.MoveNext
      DoEvents
    If pCancelar Then Err.Raise 20015, "Asignar Numeros de Lote ", " Proceso Cancelado por el Usuario "
    Loop
  End If

  AsignarNrosLotes = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  Set rtZ = Nothing
  On Error GoTo 0
  
End Function


Private Sub EstablecerLotes(iRuta As Long, iLote As Integer)
'hace la division de la ruta según los criterios indicados en el ini
'y asigna número de lote a cada carga
'La asignación se hace sobre conexiones pertenecientes al lote indicado
Dim sQ As String, sc As String
Dim rtR As Recordset, rtL As Recordset
Dim sIni As String
Dim iMax As Integer
Dim sDef As String, sDiv As String, sDivPro As String
Dim iCntCx As Long, iCntDef As Long
Dim iEst As Integer, iNewLote As Integer
Dim i As Integer, ib As Integer, j As Integer, k As Integer, p As Integer
Dim iOrDe As Integer, iSuDe As Integer
Dim iOrHa As Integer, iSuHa As Integer

  On Error GoTo errorAca
  iEst = NoCargado_CECx
  sQ = " SELECT * FROM DatosIns " & _
       " WHERE Ruta = " & iRuta & _
       " AND LoteCarga = " & iLote & _
       " AND Estado =  " & iEst & _
       " ORDER BY Orden,Suborden,Conexion "
  Set rtR = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  
  If rtR.RecordCount = 0 Then
    Set rtR = Nothing
    Exit Sub  'no hay nada para asignar
  End If
  'Hay conexiones
  rtR.MoveLast
  rtR.MoveFirst
  iCntCx = rtR.RecordCount
  'Buscar definiciones de división
  sIni = FtPhX.GetIni("Carpetas", "Dir Empresa") & "\DivisionesRuta.ini"
  sc = LeerIni("Divisiones Rutas", "Maximo", "300", sIni)
  iMax = IIf(IsNumeric(sc), Val(sc), 300)
  sDef = Trim(LeerIni("Divisiones Rutas", "Default", "D:-300", sIni))
  sDiv = Trim(LeerIni("Divisiones Rutas", Trim(iRuta), "", sIni))
  
  'buscar ultimo Nro de lote generado
  sQ = " SELECT * FROM Lotes "
  Set rtL = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  
  'si cantidad es menor que máximo y sDef="" genera un único lote
  If iCntCx <= iMax And sDiv = "" Then
    sDef = "" 'Division por defecto es "No dividir nada"
  End If
  If sDiv = "" Then sDiv = sDef   'si no tiene definicion usa division por defecto
  sDivPro = Trim(IIf(sDiv = "", sDef, sDiv))
  If Right(sDivPro, 1) <> ";" Then sDivPro = sDivPro & ";" 'para asegurar que procese el remanente
  
  'dividir y asignar Nro de lote
  Do
    i = InStr(1, sDivPro, ";", vbTextCompare)
    If i = 0 Then 'Exit Do
      sDef = ""
    Else
      sDef = Trim(Left(sDivPro, i - 1))
      sDivPro = Trim(Mid(sDivPro, i + 1))
    End If
    Select Case UCase(Left(sDef, 1))
      Case "T"    'Division Tomando Cantidad Fija -----------
        sc = Mid(sDef, 3)
        iCntDef = IIf(IsNumeric(sc), Val(sc), 0)
        sQ = " SELECT TOP " & iCntDef & _
             " * FROM DatosIns " & _
             " WHERE Ruta = " & iRuta & _
             " AND LoteCarga = " & iLote & _
             " ORDER BY Orden,Suborden,Conexion "
        
      Case "S"    'Divide por Orden-Suborden ---------------
        sc = Mid(sDef, 3)
        j = InStr(1, sc, "-", vbTextCompare)
        If j = 0 Then
          iOrHa = 9999
          iSuHa = 999
          iOrDe = Fix(sc)
          iSuDe = (Val(sc) - iOrDe) * 1000
        Else
          sc = Left(sc, j - 1)
          iOrDe = Fix(sc)
          iSuDe = (CDbl(sc) - iOrDe) * 1000
          sc = Mid(Mid(sDef, 3), j + 1)
          iOrHa = Fix(sc)
          iSuHa = (CDbl(sc) - iOrHa) * 1000
        End If
        sQ = " SELECT * FROM DatosIns " & _
             " WHERE Ruta = " & iRuta & _
             " AND LoteCarga = " & iLote & _
             " AND ((Orden = " & iOrDe & _
             " AND SubOrden >= " & iSuDe & _
             " ) OR Orden > " & iOrDe & _
             ") AND ((Orden = " & iOrHa & _
             " AND SubOrden <= " & iSuHa & _
             ") OR Orden < " & iOrHa & _
             ") ORDER BY Orden,Suborden,Conexion "
             
      Case "D"    'Divide en partes iguales y/o por cantidad maxima
        sc = Mid(sDef, 3)
        iCntDef = IIf(IsNumeric(sc), Val(sc), 0)
        p = IIf(iCntDef < 0, iCntCx \ Abs(iCntDef) + 1, iCntDef) 'partes en que debe dividirse
        k = iCntCx \ p      'cantidad de conexiones de cada parte
        k = IIf(Abs(iCntCx - p * k) <= Abs(iCntCx - p * (k + 1)), k, k + 1)
        If p > 1 Then
          For j = 2 To p
            sDivPro = "T:" & Trim(k) & ";" & sDivPro
          Next
        End If
        'para que no haga nada en la primer pasada
         sQ = " SELECT * FROM DatosIns WHERE Ruta = 0"
      Case Else
        'asigna todo lo que queda al lote
         sQ = " SELECT * FROM DatosIns " & _
              " WHERE Ruta = " & iRuta & _
              " AND LoteCarga = " & iLote & _
              " AND Estado =  " & iEst & _
              " ORDER BY Orden,Suborden,Conexion "
    End Select
    Set rtR = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
    If rtR.RecordCount > 0 Then
      'obtiene nuevo Nro lote
      rtR.MoveFirst
      rtR.MoveLast
      rtL.AddNew
      rtL.Fields("Ruta").Value = iRuta
      rtL.Fields("Cantidad").Value = rtR.RecordCount
      rtL.Fields("Hasta").Value = rtR.Fields("Orden").Value * 1000 + rtR.Fields("SubOrden").Value
      rtR.MoveFirst
      rtL.Fields("Desde").Value = rtR.Fields("Orden").Value * 1000 + rtR.Fields("SubOrden").Value
      rtL.Update
      rtL.MoveLast
      'aplica el nuevo lote
      iNewLote = rtL.Fields("Lote").Value
      Do Until rtR.EOF
        rtR.Edit
        rtR.Fields("LoteCarga").Value = iNewLote
        rtR.Update
        rtR.MoveNext
      Loop
    End If
    
    ''' dejar esto para que salga del loop
    sQ = " SELECT * FROM DatosIns " & _
     " WHERE Ruta = " & iRuta & _
     " AND LoteCarga = " & iLote & _
     " AND Estado =  " & iEst & _
     " ORDER BY Orden,Suborden,Conexion "
    Set rtR = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
    If rtR.RecordCount > 0 Then rtR.MoveLast
    '''''''
    DoEvents
    If pCancelar Then Err.Raise 20015, "Establecer Lotes ", " Proceso Cancelado por el Usuario "
  Loop Until rtR.RecordCount = 0
  
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  Set rtR = Nothing
  Set rtL = Nothing
  
End Sub


Private Function GenerarCargas() As Integer
Dim ndX As Node
Dim idx As Integer
Dim sQ As String
Dim rtC As Recordset, rtZ As Recordset
Dim totGen As Integer
Dim ib As Integer
Dim iNoLeido As Integer

  On Error GoTo errorAca
  ReDim sCargasCreadas(0)
  ReDim sCargasNOCreadas(0)
  Me.MousePointer = vbHourglass
  
  DecimalesMontoImpres = Val(FtPhX.LeerIni("Constantes Varias", "Decimales Montos Impresion", , , "2"))
  DecimalesConceptCoef = Val(FtPhX.LeerIni("Constantes Varias", "Decimales Conceptos Coeficientes", , , "4"))
  DecimalesConceptPtje = Val(FtPhX.LeerIni("Constantes Varias", "Decimales Conceptos Porcentaje", , , "2"))
  iNoLeido = FtPhX.GetIni("Novedades2", "No Leido")
  
  'la generacion se hace recorriendo el tree view y tomando las libretas
  'cuya imagen es "Todo", para enviarla al generador de una carga
  For Each ndX In tvwCargas.Nodes
    If InStr(1, ndX.Text, txRuta, vbTextCompare) > 0 And ndX.Image = "Todo" Then
      'El nodo es una ruta y está seleccionado
      idx = ndX.Index
      sQ = " SELECT * FROM DatosIns " & _
           " WHERE LoteCarga = " & infNdX(idx).Lote & _
           " AND Estado = " & NoCargado_CECx & _
           " AND Novedad2= " & iNoLeido & _
           " ORDER BY Orden,SubOrden,Conexion "
      Set rtC = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
      If rtC.RecordCount > 0 Then
        'buscar el archivo Ins
        sQ = " SELECT * FROM ArchivosIns " & _
             " WHERE Zona = " & infNdX(idx).ZonaNro
        Set rtZ = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
        If rtZ.RecordCount > 0 Then
          rtZ.MoveFirst
          ArchIns.RutNombre = rtZ.Fields("Archivo Importacion").Value
          ArchIns.Directorio = CarpetaDe(ArchIns.RutNombre)
          ndX.Selected = True
          ndX.EnsureVisible
          If GeneraUnaCarga(rtC, tvwCargas.SelectedItem) Then
            'si pudo generar agrega la carga a la lista
            ib = UBound(sCargasCreadas, 1) + 1
            ReDim Preserve sCargasCreadas(ib)
            sCargasCreadas(ib) = ndX.Text
            ndX.Image = "GenOk"
          Else
            'si NO pudo generar agrega a la lista de errores
            ib = UBound(sCargasNOCreadas, 1) + 1
            ReDim Preserve sCargasNOCreadas(ib)
            sCargasNOCreadas(ib) = ndX.Text
            ndX.Image = "Error"
          End If
        Else
          MsgBox "  No se encuentra el Archivo de importación ''Datos.Ins''   " & vbCrLf & _
                 "  para la Zona " & infNdX(idx).ZonaNro & "  para generar la carga de: " & vbCrLf & _
                 ndX.Text, vbOKOnly + vbCritical, " NO SE ENCUENTRA UN ARCHIVO Datos.Ins  "
          ndX.Image = "Error"
          ib = UBound(sCargasNOCreadas, 1) + 1
          ReDim Preserve sCargasNOCreadas(ib)
          sCargasNOCreadas(ib) = ndX.Text
        End If
      End If
      
    End If
    DoEvents
    If pCancelar Then Err.Raise 20015, "Generar Cargas ", " Proceso Cancelado por el Usuario "
  Next

  stBar.SimpleText = " Generadas " & Trim(UBound(sCargasCreadas, 1)) & _
                     " cargas correctamente"
  If UBound(sCargasNOCreadas, 1) > 0 Then
    stBar.SimpleText = stBar.SimpleText & ", y No fueron generadas las cargas de " & _
                       Trim(UBound(sCargasNOCreadas, 1)) & _
                       "  ruta/s por errores en el procesamiento  "
  End If
  GenerarCargas = totGen

errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  Set rtC = Nothing
  Set rtZ = Nothing
  Me.MousePointer = vbDefault
  
End Function

Private Function GeneraUnaCarga(rtCarga As Recordset, nodoCarga As Node) As Boolean
'El archivo Datos.Ins está en ArchIns
'Los usuarios que deben cargarse están en rtCarga ordenados
'cada tabla que se genera se carga en archivos temporales de la aplicación
Dim tabUsu As Variant
Dim sCarpTmp As String 'carpeta para los temporales
Dim sTxSt As String
Dim i As Integer
Dim bOk As Boolean
Dim sFl As File
  
  GeneraUnaCarga = False
  On Error GoTo errorAca
  sListaConceptos = ""
  sListaCategorias = ""
  If rtCarga.RecordCount = 0 Then Exit Function
  sTxSt = " Generando Carga de " & nodoCarga.Text & " - "
  nodoCarga.Image = "EnPro"
  stBar.SimpleText = sTxSt
  
  'abre el datos.ins
  ArchIns.Nro = FreeFile
  Open ArchIns.RutNombre For Random Access Read As #ArchIns.Nro Len = Len(RegIns)
  ArchIns.Size = LOF(ArchIns.Nro) \ Len(RegIns)
  
  'cargar tablas
  If Not pCancelar Then
    stBar.SimpleText = sTxSt & " Cargando Cuadro Tarifario "
    CargarCuadroTarifario ArchIns.Directorio
  End If
  If Not pCancelar Then
    stBar.SimpleText = sTxSt & " Cargando Cuadro alumbrado Público "
    CargarCuadroAlumbrado ArchIns.Directorio
  End If
  If Not pCancelar Then
    stBar.SimpleText = sTxSt & " Cargando Consumos de Bajas"
    CargarCambiosMedidor ArchIns.Directorio
  End If
  If Not pCancelar Then
    stBar.SimpleText = sTxSt & " Cargando Excepciones Generales y de Alumbrado"
    CargarExcepcionesGenerales ArchIns.Directorio
  End If
  If pCancelar Then Err.Raise 20015, "Generar Una Carga", " Proceso Cancelado por el Usuario"
  
  sCarpTmp = App.Path & "\Recursos\Temp\"
  CrearSiNoExiste sCarpTmp
  
  'generar las tablas individuales
  'borra todos los archivos dentro de la carpeta dentro de la carpeta temporal
  For Each sFl In fso.GetFolder(sCarpTmp).Files
    sFl.Delete True
  Next
  
  bOk = GeneraTablaUsuarios(rtCarga, nodoCarga, sCarpTmp)
  If bOk Then bOk = GeneraTablaIVAS(sCarpTmp)
  If bOk Then bOk = GeneraTablaTarifas(sCarpTmp)
  If bOk Then bOk = GeneraTablaConceptos(rtCarga, nodoCarga, sCarpTmp)
  If bOk Then bOk = GeneraTablaDomicilioPostal(nodoCarga, sCarpTmp)
  If bOk Then bOk = GeneraTablaNovedades(sCarpTmp)
  If bOk Then bOk = AgruparTablasDeCarga(nodoCarga, sCarpTmp)
  If bOk Then bOk = CopiarACarpetaEnvios(nodoCarga, sCarpTmp)
  If bOk Then bOk = RegistrarLoteCargado(rtCarga)
  
  
  
  GeneraUnaCarga = bOk
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  'cierra los archivos abiertos aca
  On Error Resume Next
  Close #ArchIns.Nro
  On Error GoTo 0
  
End Function


Private Function GeneraTablaUsuarios(rtCarga As Recordset, nodoCarga As Node, sCarpTmp As String) As Boolean
  Dim sUsuTmp As String, iUsuTmp As Integer 'nombre y numero temporal usuario
  Dim regUsu As TipoUsuSP
  Dim sTxSt As String
  Dim bOk As Boolean
  Dim i As Integer

    sTxSt = nodoCarga.Parent.Key & " - " & nodoCarga.Text & " - "
    GeneraTablaUsuarios = False
    sUsuTmp = sCarpTmp & "\UsuTmp.txt"
    stBar.SimpleText = sTxSt & " Generando "
    iUsuTmp = FreeFile
    Open sUsuTmp For Output As #iUsuTmp
    Close #iUsuTmp      'para borrar lo que tenia antes
    Open sUsuTmp For Random As #iUsuTmp Len = Len(regUsu)
    With rtCarga
      .MoveFirst
      bOk = True
      i = 0
      Do Until .EOF Or Not bOk
        i = i + 1
        stBar.SimpleText = sTxSt & " Generando Usuario " & Trim(i)
        ArchIns.Posicion = .Fields("PosicionIns").Value
        Get #ArchIns.Nro, ArchIns.Posicion, RegIns
        bOk = GeneraUnUsuario(regUsu, tvwCargas.SelectedItem, sCarpTmp)
        If bOk Then
          Put #iUsuTmp, , regUsu
          .Edit
          .Fields("Estado").Value = CodEstadoConex.Cargado_CECx
          .Update
        End If
        .MoveNext
        DoEvents
        If pCancelar Then Err.Raise 20015, "Generar Una Carga ", " Proceso Cancelado por el Usuario "
      Loop
    End With
    
    GeneraTablaUsuarios = True
    
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  'cierra los archivos abiertos aca
  On Error Resume Next
  Close #iUsuTmp
  On Error GoTo 0
    
End Function


Private Function GeneraUnUsuario(regUsuSP As TipoUsuSP, nodoCarga As Node, sCarpTmp As String) As Boolean
'el registro del usuario RegIns debe estar activo
'en regUsuSP vuelve el registro generado
Dim s0 As String * 1: s0 = Chr(0)
Dim s00 As String * 2: s00 = Chr(0) & Chr(0)
Dim s000 As String * 3: s000 = Chr(0) & Chr(0) & Chr(0)
Dim s0000 As String * 4: s0000 = Chr(0) & Chr(0) & Chr(0) & Chr(0)
Dim ulX As Currency
Dim lAux As Long
Dim sAux As String
Dim yAux As Currency
Dim i As Integer, j As Integer
Dim dtFecha As Date
Dim bOk As Boolean

  On Error GoTo errorAca
  bOk = True
  
  With regUsuSP
    'datos del usuario
  .Pagina = "U"             '  1 Código Interno- Página usuario = "U" = 85 = 55h
  .Status = s00                                          '  2 Bytes de Status
  .Lote = CABinarioSP(infNdX(nodoCarga.Index).Lote, 2)                                '  2 Número de LOTE
  .Secuencia = s00                                       '  2 Número de Secuencia
  .Operario = s00                                        '  2 Código de Operario
  .FactPtoVta = s00                                      '  2 Punto de Venta
  .FacturaNro = s0000                                    '  4 Número de Factura
  .Conexion = CABinarioSP(RegIns.RF01.NroConexion, 3)            '  3 Conexión
  .Libreta = CABinarioSP(RegIns.RF03(CpoCalc.Libreta).Valor, 2) '  2 Número de Libreta
  .ApellidoyNom = Trim(SacaNoAscii(RegIns.RF01.NombreRazonSocial))          ' 30 Apellido y Nombre usuario
  .Domicilio.Calle = Trim(SacaNoAscii(RegIns.RF01.Calle))
  .Domicilio.Numero = Trim(SacaNoAscii(RegIns.RF01.Numero))
  sAux = SacaNoAscii(RegIns.RF01.Piso)
  sAux = SacaSiSoloCerosOEspacios(sAux)
  .Domicilio.Piso = Trim(sAux)
  sAux = SacaNoAscii(RegIns.RF01.Dpto)
  sAux = SacaSiSoloCerosOEspacios(sAux)
  .Domicilio.Dpto = Trim(sAux)
    
  .CodPostal = CABinarioSP(RegIns.RF03(CpoCalc.CodigoPostal).Valor, 3)           '  3 Código Postal
  yAux = RegIns.RF03(CpoCalc.DNI_LE_LC_Viejo).Valor
  sAux = Trim(yAux)
  If Len(sAux) > 9 Then
    sAux = Left(sAux, Len(sAux) - 9) & Right(sAux, 1)
    .CuitSufPref = CABinarioSP(CLng(sAux), 2)     '  2 Prefijo y Sufijo CUIT
    sAux = Trim(yAux)
    sAux = Left(Right(sAux, 9), 8)
    .CuitNro = CABinarioSP(CCur(sAux), 4)          '  4 Número de CUIT
  Else
    .CuitSufPref = s00                             '  2 Prefijo y Sufijo CUIT
    .CuitNro = s0000                               '  4 Número de CUIT
  End If
  .IvaCondicion = CABinarioSP(RegIns.RF03(CpoCalc.TipoIVA).Valor, 1)      '  1 Código Condición IVA
  .DgrCondicion = CABinarioSP(RegIns.RF03(CpoCalc.CodigoDGR).Valor, 1)    '  1 Código de DGR e Ingresos Brutos
  .DebAutomCta = ObtenerCuentaDebitoAutomatico(Len(.DebAutomCta))                            ' 11 Cuenta Bancaria de Debito
  .CantCopias = CABinarioSP(RegIns.RF03(CpoCalc.CantidadCopias).Valor, 1) '  1 Cantidad de copias de facturas a imprimir (o impresas)
  .Categoria = CABinarioSP(RegIns.RF03(CpoCalc.Categoria).Valor, 1)       '  1 Categoria
  ObtieneCategorias     'no poner consumo para que venga la especial
  .CatEspecial = CABinarioSP(gDi.Categoria.eCategEspecial, 2)       '  2 Categoría Especial
  .Zona = CABinarioSP(RegIns.RF03(CpoCalc.Zona).Valor, 1)                 '  1 zona
  .PromedioA = CABinarioSP(RegIns.RF04.PROMEDIOB, 3)                      '  3 Promedio de Consumo
  .TopeCosFi = CABinarioSP(RegIns.RF03(CpoCalc.TopeCosenoFI).Valor, 1)    '  1 Tope coseno de fi para tarifas especiales
  .MedidorNroA = CABinarioSP(RegIns.RF04.NroMedidorB, 4)                  '  4 Numero de medidor
  .MedidorLetA = " "                                               '  1 Letra del Nº de Medidor ( 'E'  'T'  'M' )
  .MedidorDigA = CABinarioSP(RegIns.RF04.NroDigitosB, 1)                  '  1 Cantidad de Dígitos del Medidor
  'la fecha de lectura anterior la pone mas adelante al completar los estados del historial
  .EstadoAnteriorA = CABinarioSP(RegIns.RF04.EstadoAnteriorB, 3)          '  3 Estado Anterior de Medidor
  .FeLectActual = s00               '  2 Fecha de Lectura Actual Medidor
  .HoraLectActual = s00             '  2 Horario de Lectura Actual
  .EstadoActualA = s000             '  3 Estado Actual del Medidor
  .FactorMultipA = CABinarioSP(RegIns.RF04.FactorMultiplicacionB, 2)      '  2 Factor de Multiplicación Medidor
  .ConsumoA = s000                  '  3 Consumo en el Medidor Actual
  .TipoConsumo = " "                '  1 Tipo de Consumo del Mes 01
  .MedidorNroR = CABinarioSP(RegIns.RF04.NroMedidorR, 4)                  '  4 Numero de Medidor Reactivo
  .EstadoAnteriorR = CABinarioSP(RegIns.RF04.EstadoAnteriorR, 3)          '  3 Estado anterior Medidor Reactivo
  .EstadoActualR = s000             '  3 Estado Actual Medidor Reactivo
  .ConsumoR = s000                  '  3 Consumo Reactivo Medidor en Uso
  .Banderas = s0         '  1 Códigos Banderas: no se usa
  
  ' tipoCptConsumoSP   'Conceptos por consumo (0 es Potencia)
  .CptosConsumo(0).Consumo = CABinarioSP(RegIns.RF03(CpoCalc.PotenciaContratada).Valor, 3)   '    3 Consumo Escalón                         '
  .CptosConsumo(0).Importe = s0000    '    4 Importe Escalón                                                                        '
  .CptosConsumo(1).Consumo = s0000    '    3 Consumo Escalón                         '
  .CptosConsumo(1).Importe = s0000    '    4 Importe Escalón                                                                        '
  .CptosConsumo(2).Consumo = s0000    '    3 Consumo Escalón                         '
  .CptosConsumo(2).Importe = s0000    '    4 Importe Escalón                                                                        '
  .CptosConsumo(3).Consumo = s0000    '    3 Consumo Escalón                         '
  .CptosConsumo(3).Importe = s0000    '    4 Importe Escalón                                                                        '
  .CptosConsumo(4).Consumo = s0000    '    3 Consumo Escalón                         '
  .CptosConsumo(4).Importe = s0000    '    4 Importe Escalón                                                                        '
                     
  .CosenoFi = s0                      '  1 coseno de fi
  .RecargBFacPot = s0000              '  4 Importe Recargo por bajo factor de potencia
  .CuotaServicio = s0000              '  4 Importe Cuota se dervicio
  
  ' Armado de la tabla de conceptos
  bOk = bOk And GeneraConceptosUsuario(regUsuSP)
  
  sAux = Format(RegIns.RF01.PrimerVencimiento, "0000-00-00")
  .VtoFePrimero = CAFechaSP(CLng(CDate(sAux)))       '  2 Fecha primer Vencimiento
  .VtoDiSegundo = s0        '  1 Dias desde 1º a  Segundo Vencimiento No se usa mas
  .VtoImpPrimero = s0000    '  4 Importe a pagar en Primer Vencimiento
  .VtoRecSeg = s0000        '  4 Importe Recargo al 2º Vto
  .VtoIvReSe = s0000        '  4 IVA sobre Recargo 2º Vto
  .VtoDgReSe = s0000        '  4 DGR sobre Recargo 2º Vto
  .VtoImpoSe = s0000        '  4 Importe a pagar en Segundo Vencimiento
  .VtoProximo = Chr(28)     '  1 Dias desde 1º a Próximo vencimiento aproximado
  .Novedades = s0           '  1 Novedades
  .libre = s000             '  3 No usados por ahora
  
  If RegIns.RF03(CpoCalc.EnergiaConsorcio_Ref).Valor <> 0 Then Stop
  sAux = Format(RegIns.RF03(CpoCalc.Categoria).Valor, "-00000") & _
        ";" & Format(gDi.Categoria.sSubcategoria, "00000") & _
        ";" & Format(gDi.Categoria.eCategEspecial, "00000")
  If InStr(1, sListaCategorias, sAux) = 0 Then sListaCategorias = sListaCategorias & sAux
        
  'arma el historial de consumos
  bOk = bOk And ArmarHistorial(regUsuSP, dtFecha)
  'verifica si hay cambio de medidor (si hay crea un registor y trae la fecha)
  bOk = bOk And GeneraRegistroCambioMedidor(dtFecha, sCarpTmp)
  If bOk Then .FeLectAnterior = CAFechaSP(dtFecha)   '  2 Fecha de Lectura Anterior o Alta del Medidor
  
  'si la generacion no tubo problemas se pone el codigo de usuario como viene, en caso
  'contrario se pone el lugasr de envio=7, para NO imprimir
  lAux = IIf(bOk, RegIns.RF03(CpoCalc.LugarDeEnvio).Valor, CdgoDV.NoImprimir_CdgoDV)
  .CodigoUsu = CABinarioSP(RegIns.RF03(CpoCalc.Usuario).Valor * 10000 + _
                          RegIns.RF03(CpoCalc.SubUsuario).Valor * 10 + lAux, 4)
                          '  4 Número de Orden-Suborden/DV
  
  GeneraUnUsuario = True
 
 End With
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
      
End Function

Private Function ObtenerCuentaDebitoAutomatico(iLenCampo As Integer) As String
'con el RegIns activo, obtiene la cuenta de debito automatico en formato BCD
'El código de debito automatico debe ser distinto de 0, en ese caso
'se busca la conexion en el archivo 'debtiautomatico.txt' junto a 'datos.ins'
Dim sAr As String
Dim iAr As Integer
Dim sLinea As String
Dim sDato As String, sSal As String
Dim sSs As tipoTablaDebitoAutomatico
Dim i As Integer
Dim j As Integer
Dim sc As String
Dim vValor As Variant

  On Error GoTo errorAca
  
  ObtenerCuentaDebitoAutomatico = String(iLenCampo, 0)
    
  If (RegIns.RF03(CpoCalc.CodigoDebitoAutomatico).Valor) = 0 Then
    Exit Function
  End If
  
  'si código de debito automatico no es '0' se debe buscar en
  'el archivo de texto la conexion
  sAr = ArchIns.Directorio & "\DEBITOAUTOMATICO.TXT"
  iAr = FreeFile
  Open sAr For Input Access Read As #iAr
  sSs.Conexion_2 = "0"
  Do Until EOF(iAr)
    Line Input #iAr, sLinea
    sSs.Conexion_2 = Mid(sLinea, Len(sSs.FechaModif_1) + 1, Len(sSs.Conexion_2))
    If IsNumeric(sSs.Conexion_2) Then
      If Trim(sSs.Conexion_2) = Trim(RegIns.RF01.NroConexion) Then Exit Do
    End If
  Loop
  
  If Trim(sSs.Conexion_2) = Trim(RegIns.RF01.NroConexion) Then
    'si encontró obtiene el numero de cuenta o cbu, el que exista
    sSs.CBU_6 = Mid(sLinea, Len(sSs.FechaModif_1 & sSs.Conexion_2 & sSs.CodBanco_3 & _
                    sSs.Cuenta_4 & sSs.UltimoPeriodo_5) + 1, Len(sSs.CBU_6))
    sSs.Cuenta_4 = Mid(sLinea, Len(sSs.FechaModif_1 & sSs.Conexion_2 & sSs.CodBanco_3) + 1, _
                    Len(sSs.Cuenta_4))
    'de los dos valores toma el que tenga mayor longitud sin espacios
    sDato = IIf(Trim(sSs.CBU_6) > Trim(sSs.Cuenta_4), sSs.CBU_6, sSs.Cuenta_4)
    sDato = Trim(sDato)
    'el dato contenido en sDato se convierte a un tipo especial de BCD donde los
    'caracteres especiales se convierten a valores hexa no decimales como se indica
    '
    '         A = "*" Asterisco = Asc($2A)
    '         B = "+" Signo Mas = Asc($2B)
    '         C = "-" Guión medio = Asc($2D)
    '         D = "." Punto = Asc(2E)
    '         E = " " Espacio = Asc($)
    '         F = "/" Barra de División = Asc($2F)
    'remplaza los no numericos por estos valores
    vValor = CDec(sDato)
    For i = 1 To Len(sDato)
      Select Case Mid(sDato, i, 1)
        Case "0" To "9"
          'no modifica
        Case "*"
          Mid(sDato, i, 1) = "A"
        Case "+"
          Mid(sDato, i, 1) = "B"
        Case "-"
          Mid(sDato, i, 1) = "C"
        Case "."
          Mid(sDato, i, 1) = "D"
        Case "/"
          Mid(sDato, i, 1) = "F"
        Case Else
          'todo caso no contemplado es igual que un espacio
          Mid(sDato, i, 1) = "E"
      End Select
    Next
    'si la cantidad es impar agrega un espacio a la izquierda
    If Len(sDato) Mod 2 < 0 Then sDato = "E" & sDato
    
    'toma de a dos como sio fueran hexa
    sSal = ""
    For i = 1 To Len(sDato) Step 2
      j = "&H" & Mid(sDato, i, 2)
      sSal = sSal & Chr(j)
      sc = Hex(j)
    Next

    ObtenerCuentaDebitoAutomatico = sSal

  End If

errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Close #iAr
  On Error GoTo 0
  
End Function


Private Function ArmarHistorial(regUsuSP As TipoUsuSP, dtFecha1 As Date) As Boolean
'completa el arreglo siguiente
'  .Estados(1 To 11)   ' TipoEstadosSP     'Historial de estados anteriores
                      '    3 Estado del medidor
                      '    2 Fecha de lectura
                      '    3 Consumo en KWh
                      '    2 Fecha de 1er vencimiento
                      '    2 Fecha de pago
  ''''''''''''''''''''''''''''''''''''''''
 'donde la posicion 1 corresponde al periodo anterior y asi sucesivamente
  Dim dtPeri As Date, dtFech As Date
  Dim idXp As Integer
  Dim i As Integer
  Dim dtX As Date
  Dim lF As Long
  
  On Error GoTo errorAca
  ArmarHistorial = False
  
  dtPeri = CDate(Format(RegIns.RF03(CpoCalc.Periodo).Valor, "0000-00") & "-15")
  
  idXp = Month(dtPeri)
  
  For i = 1 To 11
    dtX = dtPeri - 30 * i
    idXp = Month(dtX)
    With regUsuSP.Estados(i)
      .Consumo = CABinarioSP(RegIns.RF04.Consumo(idXp), 3)
      .Estado = CABinarioSP(RegIns.RF04.Lectura(idXp), 3)
      If IsNumeric(RegIns.RF04.Fecha(idXp).Fecha) Then
        lF = RegIns.RF04.Fecha(idXp).Fecha
      Else
        lF = 0
      End If
      If lF > 20000000 Then
        dtFech = CDate(Format(lF, "0000-00-00"))
        .FeLectura = CAFechaSP(dtFech)
        If i = 1 Then dtFecha1 = dtFech
      Else
        .FeLectura = Chr(0) & Chr(0)
        If i = 1 Then dtFecha1 = 0
      End If
      lF = RegIns.RF03(CpoCalc.FechaDePago1 + idXp - 1).Valor
      If lF > 20000000 Then
        .FePago = CAFechaSP(CDate(Format(lF, "0000-00-00")))
      Else
        .FePago = Chr(0) & Chr(0)
      End If
      lF = RegIns.RF03(CpoCalc.FechaVencimiento1 + idXp - 1).Valor
      If lF > 20000000 Then
        .FeVto = CAFechaSP(CDate(Format(lF, "0000-00-00")))
      Else
        .FeVto = Chr(0) & Chr(0)
      End If
    End With
  Next

  ArmarHistorial = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  
End Function


Private Function GeneraConceptosUsuario(regUsuSP As TipoUsuSP) As Boolean
'Los conceptos derivados del consumo no estan aca
'Se generan los conceptos y se ordenan por orden de impresion
'  .Conceptos(1 To 13) ' tipoConceptosSP 'Otros conceptos
                      '    2 codigo del concepto
                      '    6 Texto Adicional
                      '    4 Importe
  ''''''''''''''''''''''''''''''''''''''''
Dim bOk As Boolean
Dim dbCpt As Database
Dim rtCpt As Recordset
Dim sQ As String
Dim sCx As String, sArBd As String

'si al generar cualquiera de los conceptos devuelve falso
'la conexion se marca como de "NO impresión"
On Error GoTo errorAca

  'abre la base de conceptos
  sArBd = App.Path & "\Recursos\ExpCnfg.mdb"
  sCx = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & _
          sArBd & ";Mode=ReadWrite|Share Deny None;" & _
         "Persist Security Info=False;" & _
         "Jet OLEDB:Database Password=generaconceptos"

  Set dbCpt = OpenDatabase(sArBd, , ReadOnly)
  sQ = " SELECT * FROM [Conceptos En Campo Calculo] ORDER BY [Codigo GCA] "
  Set rtCpt = dbCpt.OpenRecordset(sQ, dbOpenDynaset)
  If rtCpt.RecordCount = 0 Then
    pCancelar = True
    Err.Raise 20050, "Genera Conceptos De Usuario", _
                    "  No se encontró la tabla de conceptos " & vbCrLf & _
                    "  o la misma está vacía " & vbCrLf & _
                    "  EL PROCESO SERÁ CANCELADO "
    
    Exit Function
  End If
  

    Lvw.ListItems.Clear
    bOk = True
''------------ALUMBRADO PUBLICO
    bOk = bOk And ConceptoAlumbradoPublico()
    
'------------APORTES JUBILATORIOS
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.RecupJubil, CptoGCA.RecupJubil, _
                                 CpoCalc.PrtjeAporteJubilados, tipoCoeficiente)

'------------CONCEPTOS IVAS Y DGR
    'IVA general
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.Iva1, CptoGCA.IvaGral, _
                                 CpoCalc.PrtjeIVA, tipoCoeficiente)
    'IVA recargo
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.Iva2, CptoGCA.IvaRecar, _
                                 CpoCalc.PrtjeRecargoIVA, tipoCoeficiente)
    'IVA percepcion
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.IvaPerc, CptoGCA.IvaPerce, _
                                 CpoCalc.PrtjePercepcionIVA, tipoCoeficiente)
    'DGR
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.DGR, CptoGCA.DGR, _
                                 CpoCalc.PrtjeIngresosBrutos, tipoCoeficiente)

'------------Impuesto nacional 0,6% ley 23681 --------------
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.Ley23691, CptoGCA.Ley23681, _
                                 CpoCalc.PrtjeLey23681, tipoCoeficiente)
'
'------------Impuesto Ley Nacional 25957 --------------
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.Ley25957, CptoGCA.Ley25957, _
                                 CpoCalc.PrtjeLey25957, tipoPorcentaje)
'          concep8$ = concep8$ + U_090_RUT_Informados(REG_MAEFAC$, 160, "IV07", cpTipoCoefic, CBool(Cancelar%))
'
'------------Impuesto Resolucin S.E.1866/05 --------------
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.ResSE1866, CptoGCA.Res1866, _
                                 CpoCalc.PrtjeRes1866, tipoPorcentaje)
'          concep8$ = concep8$ + U_090_RUT_Informados(REG_MAEFAC$, 170, "IV08", cpTipoCoefic, CBool(Cancelar%))
'
'------------Impuesto Intereses por Mora  --------------
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.InterMora, CptoGCA.IntMora, _
                                 CpoCalc.InteresesPorMora, tipoMonto)
'          concep9$ = U_090_RUT_Informados(REG_MAEFAC$, 460, "IV09", cpTipoMonto, CBool(Cancelar%))
'
'------------ Servicio de Agua    --------------
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.ServAgua, CptoGCA.ServAgua, _
                                 CpoCalc.ServicioAgua, tipoMonto)
'          concep10$ = U_090_RUT_Informados(REG_MAEFAC$, 304, "IV10", cpTipoMonto, CBool(Cancelar%))
'
'------------PARTIDA MUNICIPAL
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.TasaGralI, CptoGCA.TaGralInm, _
                                 CpoCalc.TasaGeneralInmuebles, tipoMonto, _
                                 CpoCalc.NroPartidaInmobiliaria)

'------------CONCEPTOS DEBITOS GRAVADOS
'          concep5$ = U_090_RUT_DEBITOS(REG_MAEFAC$, REG_DEB, CBool(Cancelar%))
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.DebVsGrav, CptoGCA.DebVcIva, _
                                 CpoCalc.DebitosVariosGravados, tipoMonto, , True)

'------------CONCEPTOS DEBITOS NO GRAVADOS
    bOk = bOk And ConceptosInformadosEnCpoCalc(CptoEmsa.DevVsNoGra, CptoGCA.DebVsIva, _
                                 CpoCalc.DebitosVariosNoGravados, tipoMonto, , True)


'          '-------------------------------------------------------
'
'          '------------Otros Conceptos --------------
'          concep6$ = U_090_RUT_CONCEPTOS_GRALES(REG_MAEFAC$, CBool(Cancelar%))
'
'          '------------ENERGIA CONSORCIO TABECO --------------
'          concep7$ = U_090_rut_ENERGIA_CONSORCIO(REG_MAEFAC$, ARC_TABECO%, CBool(Cancelar%))
'

'

'------------CONCEPTOS CREDITOS
'         no se cargan Mas los créditos

  
  GeneraConceptosUsuario = bOk = bOk And ArmarConceptosUsuario(regUsuSP.Conceptos)

errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Set dbCpt = Nothing
  Set rtCpt = Nothing
  On Error GoTo 0
      
End Function


Private Function ConceptosInformadosEnCpoCalc(CodCptoEmsa As Integer, CodCptoGCA As Integer, _
                                     CampoCalcDato As CpoCalc, TipoMonCoePj As TipoCpto, _
                                     Optional CpoCalcTextAdic As CpoCalc = -1, _
                                     Optional PonerCodigo0siNoAplica As Boolean = False) As Boolean
'Arma los conceptos que son informados en campos de calculo
'PonerCodigo0sinoAplica: si es true y el importe es 0  se
'pone en cero el código para que no sea generado
Dim Importe As Double
Dim Concep As Boolean
Dim Desc As String
Dim iCodCpto As Integer
    
    On Error GoTo errorAca
    
    Concep = True
    ConceptosInformadosEnCpoCalc = False
    
    iCodCpto = CodCptoGCA
    Importe = RegIns.RF03(CampoCalcDato).Valor
    If CpoCalcTextAdic = -1 Then
      ' el concepto es un iva, o dgr se le agrega como descripcion enl porcentaje
      If CampoCalcDato = PrtjeIVA Or CampoCalcDato = PrtjeIngresosBrutos Or _
         CampoCalcDato = PrtjePercepcionIVA Or CampoCalcDato = PrtjeRecargoIVA Then
        Desc = Trim(RegIns.RF03(CampoCalcDato).Valor) & "%"
      Else
        Desc = ""
      End If
    Else
      Desc = Trim(RegIns.RF03(CpoCalcTextAdic).Valor)
    End If
    Desc = Format(Desc, "!@@@@@@")
    
    VerExcepcionesGenerales CodCptoEmsa, iCodCpto, Importe, TipoMonCoePj
    If Importe = 0 And PonerCodigo0siNoAplica Then iCodCpto = 0
    If iCodCpto <> 0 Then
       Concep = AgregaConceptoALista(iCodCpto, Desc, Importe, TipoMonCoePj)
    End If
    
    ConceptosInformadosEnCpoCalc = Concep
    
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0

End Function


Private Function AgregaConceptoALista(ByVal CodCptoGCA As Variant, _
                            ByVal TextoAdic As String, ByVal Importe As Variant, _
                            uTipoMonCoePj As TipoCpto, _
                            Optional Dividir As Boolean = True) As Boolean
Dim sQ As String
Dim rtC As Recordset
Dim iOrd As Integer
Dim itmX As ListItem
'OJO: Los conceptos IVAS y similares están en codigos del 500 al 599

    AgregaConceptoALista = False
    On Error GoTo errorAca
        
    '''''''''''''''''''''''''''''''''''''''''''''''''
    'buscar el orden de impresion
    sQ = " SELECT * FROM  Conceptos " & _
         " WHERE Codigo = " & CodCptoGCA
    Set rtC = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
    If rtC.RecordCount > 0 Then
      rtC.MoveFirst
      iOrd = rtC.Fields("Orden").Value
      sQ = rtC.Fields("Descripcion").Value
      If rtC.Fields("Grabado_IVA").Value = 0 _
       And (CodCptoGCA \ 100 <> 5) Then iOrd = iOrd + 10000  'asume que los ivas están en 500 a 599
    End If
      
    Set itmX = Lvw.ListItems.Add
    itmX.Text = Format(iOrd, "0-0000")
    itmX.SubItems(1) = CodCptoGCA
    itmX.SubItems(2) = TextoAdic
    itmX.SubItems(3) = Importe
    itmX.SubItems(4) = uTipoMonCoePj
    itmX.SubItems(5) = sQ
    Set rtC = Nothing
     ''''''''''''''''''''''''''''''''''''''''''''''''
    
    AgregaConceptoALista = True  ' Parte1 & Parte2 & Parte3
    
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  Set rtC = Nothing
  On Error GoTo 0

End Function


Private Function ArmarConceptosUsuario(Conceptos() As tipoConceptosSP) As Boolean
'carga desde la lista Lvw los conceptos en la matriz de conceptos para la SP
Dim i As Integer, k As Integer, uK As Integer
Dim iCod As Integer
Dim sTxt As String
Dim dImporte As Double
Dim uTipo As TipoCpto
Dim Decimales As Integer

  ArmarConceptosUsuario = False
  On Error GoTo errorAca
  
  k = 0
  For k = 1 To Lvw.ListItems.Count
    With Lvw.ListItems(k)
      iCod = .SubItems(1)
      sTxt = Format(" " & Trim(.SubItems(2)), "!@@@@@@")
      dImporte = .SubItems(3)
      uTipo = .SubItems(4)
      Decimales = IIf(uTipo = tipoMonto, DecimalesMontoImpres, DecimalesConceptCoef)
      dImporte = IIf(uTipo = tipoCoeficiente, dImporte / 100, dImporte)
            
      Conceptos(k).Codigo = CABinarioSP(iCod, 2)
      Conceptos(k).TextoAd = sTxt
      Conceptos(k).Importe = CABinarioSP(dImporte, 4, Decimales)
    End With
    'si no está el concepto en la lista lo agrega
    If InStr(1, sListaConceptos, Format(iCod, sFormatoListaConceptos), vbTextCompare) = 0 Then
      sListaConceptos = sListaConceptos + Format(iCod, sFormatoListaConceptos)
    End If
  Next
  'completa los conceptos que quedan con nulos
  uK = UBound(Conceptos, 1)
  For i = k To uK
    Conceptos(i).Codigo = String(2, 0)
    Conceptos(i).TextoAd = String(6, " ")
    Conceptos(i).Importe = String(4, 0)
  Next
  
  'i = Len(sListaConceptos)
  
  ArmarConceptosUsuario = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error GoTo 0
  
End Function


Private Function ConceptoAlumbradoPublico() As Boolean
' En Campos de Calculo...
  '   Si "Codigo Aumbrado Publico"
      '     = 0: no hay alumbrado se devuelve vacio
      '     = 1: Alumbrado es un Monto que se saca de tabla segun el
      '           código postal, categoria  y consumo
      '     = 2: Es un porcentaje que se extrae de tabla segun el
      '           código postal, categoria y consumo
      '     = 3: Es un porcentaje informado en "Porcentaje Alumbrado Publico"

'el concepto vuelve en TmpCptSp(indiceEnCptoSP)
Dim CodApConex As Integer
Dim CPb As Long
Dim Encontro As Boolean
Dim pTipo As TipoCpto
Dim rtA As Recordset
Dim sQ As String
Dim bOk As Boolean

Const BaseCodApMonto As Integer = 600
Const BaseCodApCoef As Integer = 800

'para devolver '''''''''''''
Dim CodConc As Integer
Dim Importe As Double
Dim LineaConcepto As String
''''''''''''''''''''''''''''
  
  On Error GoTo errorAca
  
  ConceptoAlumbradoPublico = False
  bOk = True
  CPb = RegIns.RF03(CpoCalc.CodigoPostal).Valor
  CodApConex = RegIns.RF03(CpoCalc.CodigoAlumbradoPublico).Valor
  
  Select Case CodApConex
    Case 0    'No se cobra AP
      Exit Function
    Case 1, 2 'Valores de tabla
      'los valores se extraen de tabla.
      'si CodAP=1 son montos, si es 2 son porcentajes, se debe
      'extraer el código que corresponde segun codigo postal y categoria
      'de la tabla ALUMBRADO_TABLA
      sQ = " SELECT * FROM [Alumbrado Publico] " & _
           " WHERE [Codigo Postal] = " & CPb & _
           " AND Categoria = " & RegIns.RF03(CpoCalc.Categoria).Valor
      Set rtA = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
      If rtA.RecordCount = 0 Then
        CPb = 3300
        'si no encuentra usa el de posadas
        sQ = " SELECT * FROM [Alumbrado Publico] " & _
             " WHERE [Codigo Postal] = " & CPb & _
             " AND Categoria = " & RegIns.RF03(CpoCalc.Categoria).Valor
        Set rtA = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
      End If
      If rtA.RecordCount = 0 Then
        MsgBox " Falta la Tabla de Alumbrado Público " & vbCrLf & _
               " o falta el registro para Codigo Postal " & _
                   RegIns.RF03(CpoCalc.CodigoPostal).Valor & _
                   " - Categoria " & RegIns.RF03(CpoCalc.Categoria).Valor & vbCrLf & _
               " ¡¡ SE MARCARÁ ESTA CONEXIÓN COMO ''NO IMPRIMIBLE''  ", _
               vbOKOnly + vbCritical, "Falta Tabla de alumbrado Público"
        'se marca esta conexion como no imprimible
        bOk = False
      End If
      
      CodConc = 0: Importe = 0
      If rtA.RecordCount > 0 Then
        rtA.MoveLast: rtA.MoveFirst
        If rtA.RecordCount = 1 Then
          'si hay un solo registro, es calor único se informa
          CodConc = 0 'para que despues aplique lo que corresponda
          Importe = rtA.Fields("Valor").Value
        Else
          'hay mas de un registro, buscar su código en la tabla de códigos
          sQ = " SELECT * FROM [Alumbrado Codigos] " & _
               " WHERE [Codigo Postal] = '" & Trim(CPb) & _
               "' AND Categoria = " & RegIns.RF03(CpoCalc.Categoria).Valor
          Set rtA = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
          If rtA.RecordCount > 0 Then
            rtA.MoveFirst
            CodConc = rtA.Fields("Codigo Concepto").Value
          Else
            MsgBox " Falta la Tabla de Alumbrado Público " & vbCrLf & _
                  " o falta el registro para Codigo Postal " & _
                      RegIns.RF03(CpoCalc.CodigoPostal).Valor & _
                      " - Categoria " & RegIns.RF03(CpoCalc.Categoria).Valor & vbCrLf & _
                  " ¡¡ SE MARCARÁ ESTA CONEXIÓN COMO ''NO IMPRIMIBLE''  ", _
                  vbOKOnly + vbCritical, "Falta Tabla de alumbrado Público"
            'se marca esta conexion como no imprimible
            bOk = False
          End If
        End If
        CodConc = IIf(CodConc = 0, BaseCodApMonto, CodConc)
        CodConc = IIf(CodApConex = 1, CodConc, CodConc + BaseCodApCoef - BaseCodApMonto)
      End If
    Case 3    'Porcentaje Informado
      'El porcentaje a aplicar se obtiene de PorAP y se asigna el
      'código 800, donde estan las demas condiciones.-
      Importe = RegIns.RF03(CpoCalc.PrtjeAlumbradoPublico).Valor
      CodConc = 800
    Case Else
      MsgBox "Error en Codigo de Alumbrado Público"
      'Poner DV=7   #######
      bOk = False
  End Select
    
  Select Case CodApConex
    Case 1: pTipo = tipoMonto
    Case 2: pTipo = tipoCoeficiente
    Case 3: pTipo = tipoCoeficiente
  End Select
    
  If Not VerExcepcionesGenerales(CptoEmsa.AlumbPub, CodConc, Importe, pTipo) Then
    MsgBox "  ERROR al tratar las excepciones del alumbrado Publico " & vbCrLf & _
           "  Concexión: " & RegIns.RF01.NroConexion & vbCrLf & _
           "  Libreta: " & RegIns.RF03(CpoCalc.Libreta).Valor & vbCrLf & _
           " Se marcará la conexión como ''NO IMPRIMIBLE''  "
    'El Alumbrado tiene su propio tratamiento de excepciones, dejarlo por su lado
    bOk = False
  End If
 
  ConceptoAlumbradoPublico = bOk And AgregaConceptoALista(CodConc, " ", Importe, pTipo)
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  
  Set rtA = Nothing
  On Error GoTo 0
  
End Function


Private Function VerExcepcionesGenerales(CodCptoEmsa As Integer, CodCptoGCA As Integer, _
                                         dlValor As Double, uTipo As TipoCpto) As Boolean
'Verifica si hay alguna causal de excepcion.-
Dim sQ As String
Dim rtEx As Recordset

'las excepciones estan en la tabla Excepciones Generales
On Error GoTo 0
    VerExcepcionesGenerales = False
    
    sQ = " SELECT * FROM [Excepciones Generales] " & _
         " WHERE Concepto = " & CodCptoEmsa & _
         " AND [Desde Fecha] <= " & CDate(Format(Now, "dd/mm/yyyy")) & _
         " AND [Hasta Fecha] >= " & CDate(Format(Now, "dd/mm/yyyy")) & _
         " AND (Conexion = " & CLng(RegIns.RF01.NroConexion) & _
               " OR CUIT = " & RegIns.RF03(CpoCalc.DNI_LE_LC_Viejo).Valor & _
               " OR Ruta = " & RegIns.RF03(CpoCalc.Libreta).Valor & ")"
    Set rtEx = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
          
    If rtEx.RecordCount > 0 Then
      'hay una excepcion, ver si es monto o porcentaje
      If rtEx.Fields("Porcentaje").Value <> 0 Then
        'si hay un valor en porcentaje, asigna este
        dlValor = rtEx.Fields("Porcentaje").Value
        uTipo = tipoCoeficiente
        If CodCptoEmsa = CptoEmsa.AlumbPub Then CodCptoGCA = 800  'Porcentaje informado, si es AP
      Else
        'si no hay nada en porcentaje asigna lo de monto aunque sea cero
        dlValor = rtEx.Fields("Monto").Value
        uTipo = tipoMonto
        If CodCptoEmsa = CptoEmsa.AlumbPub Then CodCptoGCA = 600  'Monto informado, si es alimbrado
        If dlValor = 0 Then CodCptoGCA = 0 'para que no carge el concepto
      End If
    End If
    
    VerExcepcionesGenerales = True

errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  
  Set rtEx = Nothing
  On Error GoTo 0

End Function


Private Function GeneraTablaConceptos(rtCarga As Recordset, nodoCarga As Node, sCarpTmp As String) As Boolean
'Genera una tabla de conceptos con los conceptos que estan en la sListaConceptos
Dim sLstCp() As String
Dim i As Integer, ib As Integer
Dim iCpt As Integer
Dim sTxSt As String
Dim rtCp As Recordset
Dim rtEs As Recordset
Dim sQ As String, sIj As String
Dim iCptTmp As Integer, sCptTmp As String
Dim regCpt As tipoTablaConceptosCabezSP
Dim uTipo As TipoCpto
Dim iDecimales As Integer
Dim sRegSal As String
Dim bOk As Boolean
Dim iCntEsc As Integer
Dim lPos As Long

  On Error GoTo errorAca
  GeneraTablaConceptos = False
  
  'archivo temporal conceptos
  iCptTmp = FreeFile
  sCptTmp = sCarpTmp & "\CptCabTmp.txt"
  Open sCptTmp For Binary As #iCptTmp 'Len = LenSectorSP
  
  ReDim sLstCp(0)
  sLstCp() = Split(sListaConceptos, "-", -1, vbTextCompare)
  sTxSt = nodoCarga.Parent.Key & " - " & nodoCarga.Text & " - Generando Tabla Conceptos - "
  stBar.SimpleText = sTxSt
  DoEvents
  
  sQ = " SELECT * FROM Conceptos WHERE "
  ib = UBound(sLstCp, 1)
  sIj = " "
  For i = 0 To ib
    iCpt = 0
    If IsNumeric(sLstCp(i)) Then iCpt = CInt(sLstCp(i))
    sQ = sQ & sIj & " Codigo = " & iCpt
    sIj = " OR "
  Next
  Set rtCp = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  If rtCp.RecordCount > 0 Then
    rtCp.MoveLast
    rtCp.MoveFirst
    ib = rtCp.RecordCount
    i = 0
    Do Until rtCp.EOF
      i = i + 1
      stBar.SimpleText = sTxSt & Trim(i + 1) & "  de " & Trim(ib + 1)
      DoEvents
      With regCpt
        .Pagina = "C"
        .Codigo = CABinarioSP(rtCp.Fields("Codigo").Value, Len(.Codigo))
        sQ = " SELECT * FROM [Conceptos Escalones] WHERE Cod_Concepto = " & rtCp.Fields("Codigo").Value
        Set rtEs = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
        If rtEs.RecordCount > 0 Then rtEs.MoveFirst: rtEs.MoveLast
        iCntEsc = rtEs.RecordCount
        .CantEsc = CABinarioSP(iCntEsc, Len(.CantEsc))
        .Signo = CABinarioSP(rtCp.Fields("Signo").Value, Len(.Signo))
        .txDescrip = Trim(rtCp.Fields("Descripcion").Value)
        uTipo = rtCp.Fields("Tipo").Value
        .TipoCpto = CABinarioSP(uTipo, Len(.TipoCpto))
        iDecimales = IIf(uTipo = tipoCoeficiente, DecimalesConceptCoef, DecimalesMontoImpres)
        .Valor = CABinarioSP(rtCp.Fields("Valor").Value, Len(.Valor), iDecimales)
        .BaseCalc = CABinarioSP(rtCp.Fields("Base_Calculo").Value, Len(.BaseCalc))
        .GravadoIva = CABinarioSP(rtCp.Fields("Grabado_Iva").Value, Len(.GravadoIva))
        sRegSal = .Pagina & .Codigo & .CantEsc & .Signo & .Valor & _
                  .txDescrip & .TipoCpto & .BaseCalc & .GravadoIva
        
      End With
      lPos = LOF(iCptTmp)
        Put #iCptTmp, lPos + 1, sRegSal
      bOk = True
      If iCntEsc > 0 Then
        bOk = GeneraTablaEscalonesConceptos(rtCp.Fields("Codigo").Value, uTipo, iCptTmp)
      End If
      If Not bOk Then Exit Do
      rtCp.MoveNext
    Loop
  End If
      
  GeneraTablaConceptos = bOk

errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Close #iCptTmp
  Set rtCp = Nothing
  On Error GoTo 0

End Function


Private Function GeneraTablaEscalonesConceptos(CodCpto As Integer, _
                                  uTipoCpto As TipoCpto, iNroArch As Integer) As Boolean
'Genera una tabla de conceptos con los cocneptos que estan en la sListaConceptos
'Los escalones del concepto son agregados inmediatamente despues de la cabecera
Dim i As Integer, ib As Integer
Dim rtCp As Recordset
Dim sQ As String, sIj As String
Dim regCpt As tipoTablaConceptosEscalSP
Dim iDecimales As Integer
Dim iEscTmp As Integer, sEscTmp As String
Dim sEscSal As String
Dim lPos As Long

  On Error GoTo errorAca
  GeneraTablaEscalonesConceptos = False
  
  'archivo temporal escalones
  iEscTmp = iNroArch
   
  sQ = " SELECT * FROM [Conceptos Escalones] " & _
       " WHERE Cod_Concepto = " & CodCpto
  Set rtCp = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  If rtCp.RecordCount > 0 Then
    rtCp.MoveLast
    rtCp.MoveFirst
    ib = rtCp.RecordCount
    i = 0
    Do Until rtCp.EOF
      i = i + 1
      With regCpt
        .Pagina = "C"
        .Codigo = CABinarioSP(rtCp.Fields("Cod_Concepto").Value, Len(.Codigo))
        .Escalon = CABinarioSP(rtCp.Fields("Orden_Escala").Value, Len(.Escalon))
        .Signo = CABinarioSP(rtCp.Fields("Signo").Value, Len(.Signo))
        iDecimales = IIf(uTipoCpto = tipoCoeficiente, DecimalesConceptCoef, DecimalesMontoImpres)
        .Valor = CABinarioSP(rtCp.Fields("Valor").Value, Len(.Valor), iDecimales)
        .CalcuDesde = CABinarioSP(rtCp.Fields("Calcular_Desde").Value, Len(.CalcuDesde))
        .CalcuHasta = CABinarioSP(rtCp.Fields("Calcular_Hasta").Value, Len(.CalcuHasta))
        .AplicDesde = CABinarioSP(rtCp.Fields("Aplicar_Desde").Value, Len(.AplicDesde))
        .AplicHasta = CABinarioSP(rtCp.Fields("Aplicar_Hasta").Value, Len(.AplicHasta))
        sEscSal = .Pagina & .Codigo & .Escalon & .Signo & .Valor _
                & .CalcuDesde & .CalcuHasta & .AplicDesde & .AplicHasta & Space(7)
      End With
      lPos = LOF(iEscTmp)
      Put #iEscTmp, lPos + 1, sEscSal
      rtCp.MoveNext
    Loop
  End If
  
  GeneraTablaEscalonesConceptos = True

errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Set rtCp = Nothing
  On Error GoTo 0

End Function


Private Function GeneraTablaIVAS(sCarpTmp As String) As Boolean
'Genera tabla de condiciones de iva. Es la mimsa para todos
Dim rtV As Recordset
Dim sQ As String
Dim iVTmp As Integer, sVTmp As String
Dim sRegSal As String
Dim bOk As Boolean
Dim regIva As tipoTablaIvasSP
Dim lPos As Long

  On Error GoTo errorAca
  GeneraTablaIVAS = False
  
  'archivo temporal conceptos
  iVTmp = FreeFile
  sVTmp = sCarpTmp & "\IvaTmp.txt"
  Open sVTmp For Binary As #iVTmp 'Len = LenSectorSP
 
  sQ = " SELECT * FROM Ivas "
  Set rtV = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  If rtV.RecordCount = 0 Then
    Err.Raise 20020, " Generar Tablas IVA ", "No hay registros en la Tabla de IVA o no existe la misma"
  End If
  rtV.MoveFirst
  Do Until rtV.EOF
    With regIva
      .Pagina = "V"
      .Codigo = CABinarioSP(rtV.Fields("Codigo").Value, Len(.Codigo))
      .Condicion = Trim(rtV.Fields("Condicion").Value)
      .Signo = String(Len(.Signo), 0)
      .Alicuota_1 = CABinarioSP(rtV.Fields("Alicuota_1").Value, Len(.Alicuota_1), 2)
      .Alicuota_2 = CABinarioSP(rtV.Fields("Alicuota_2").Value, Len(.Alicuota_2), 2)
      .Percepcion = CABinarioSP(rtV.Fields("Percepcion").Value, Len(.Percepcion), 2)
      .LetraFact = Trim(rtV.Fields("Letra_Factura").Value)
      .libre = " "
      sRegSal = .Pagina & .Codigo & .Condicion & .Signo & .Alicuota_1 & _
                .Alicuota_2 & .Percepcion & .LetraFact & .libre
    End With
    lPos = LOF(iVTmp)
    Put #iVTmp, lPos + 1, sRegSal
    rtV.MoveNext
  Loop
  
  GeneraTablaIVAS = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Set rtV = Nothing
  Close #iVTmp
  On Error GoTo 0

End Function


Private Function GeneraTablaTarifas(sCarpTmp As String) As Boolean
'generar la tabla de tarifas
Dim i As Integer, ib As Integer, j As Integer
Dim rtT As Recordset
Dim sQ As String
Dim iTTmp As Integer, sTTmp As String
Dim sRegSal As String
Dim bOk As Boolean
Dim regTar As tipoTablaTarifasSP
Dim sSep() As String, sSub() As String
Dim itX As ListItem
Dim iCat As Integer, iSubCat As Integer, iCatEsp As Integer
Dim bFinCat As Boolean
Dim iDesde(5) As Integer
Dim dPrecio(5) As Double
Dim iAcumEsc As Integer
Dim lPos As Long

  On Error GoTo errorAca
  GeneraTablaTarifas = False
  
  'archivo temporal conceptos
  iTTmp = FreeFile
  sTTmp = sCarpTmp & "\TarTmp.txt"
  Open sTTmp For Binary As #iTTmp 'Len = LenSectorSP
 
  ReDim sSep(0)
  sSep() = Split(sListaCategorias, "-", -1, vbTextCompare)
  ib = UBound(sSep, 1)
  
  Lvw.ListItems.Clear
  For i = 0 To ib
    ReDim sSub(0)
    sSub() = Split(sSep(i), ";", -1, vbTextCompare)
    If UBound(sSub, 1) > 1 Then
      Set itX = Lvw.ListItems.Add
      itX.Text = Format(CInt(sSub(0)) * 1000 + CInt(sSub(1)), "000000")
      itX.SubItems(1) = CInt(sSub(0))
      itX.SubItems(2) = CInt(sSub(1))
      itX.SubItems(3) = CInt(sSub(2))
      itX.Key = "s " & itX.Text
    End If
  Next
  'Si se cargó alguna categoria sin haberse cargado su subcategoria 1,
  'se agrega tambien un intem con subcategoria 1
  For i = 0 To ib
    ReDim sSub(0)
    sSub() = Split(sSep(i), ";", -1, vbTextCompare)
    If UBound(sSub, 1) > 1 Then
      If CInt(sSub(1)) <> 1 Then
        On Error Resume Next
        sQ = Lvw.ListItems.Item("s " & Format(CInt(sSub(0)) * 1000 + 1, "000000"))
        If Err.Number > 0 Then
          On Error GoTo errorAca
          Set itX = Lvw.ListItems.Add
          itX.Text = Format(CInt(sSub(0)) * 1000 + 1, "000000")
          itX.SubItems(1) = CInt(sSub(0))
          itX.SubItems(2) = 1
          itX.SubItems(3) = 0
          itX.Key = "s " & itX.Text
        End If
        On Error GoTo errorAca
      End If
    End If
  Next
  
  sQ = "select distinct Categoria,subcategoria from Tarifas where categoria < 16 "
  
  For j = 1 To Lvw.ListItems.Count
    iCat = Lvw.ListItems(j).SubItems(1)
    iSubCat = Lvw.ListItems(j).SubItems(2)
    iCatEsp = Lvw.ListItems(j).SubItems(3)
    
    sQ = " SELECT * FROM Tarifas " & _
         " WHERE Categoria = " & iCat & _
         " AND SubCategoria = " & iSubCat & _
         " ORDER BY Escalon "
    Set rtT = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
    If rtT.RecordCount = 0 Then
      Err.Raise 20020, " Generar Tablas Tarifas ", _
                       " No hay registros en la Tabla de Tarifas " & vbCrLf & _
                       " para la Categoría: " & Trim(iCat) & "  SubCategoría: " & Trim(iSubCat) & vbCrLf & _
                       " o no existe la Tabla de Tarifas "
    End If
    rtT.MoveLast
    rtT.MoveFirst
    With regTar
      'inicializa
      For i = 0 To 5
        iDesde(i) = 0
        dPrecio(i) = 0
      Next
      'cuota de servicio se repite en todos los escalones, tomar el primero que aparece
      .CuotServ = CABinarioSP(rtT![Cuota Servicio], Len(.CuotServ), DecimalesMontoImpres)
      'potencia para categorias que lo tienen es escalon 1
      ib = IIf(iCat = 10 Or iCat = 12 Or iCat > 15, 1, 0)
      Do Until rtT.EOF
        i = rtT!Escalon - ib
        iDesde(i) = IIf(rtT![Rango Consumo] < 32766, rtT![Rango Consumo], 32766)
        dPrecio(i) = rtT!Precio
        rtT.MoveNext
      Loop
      'acomoda los valores limites,
      'desde el escalon 4 hacia el 1, el primero que tenga un valor distinto de 0
      'es el últimoescalon con límite, los demas se obtienen restando el limite del
      'anterior menos el que tiene él en ese momento
      iAcumEsc = 0
      For i = 4 To 1 Step -1
        If iDesde(i) > 0 Or iAcumEsc <> 0 Then
          If iAcumEsc <> 0 Then
            'ya habia escalones con valor
            iDesde(i) = iAcumEsc - iDesde(i)
          End If
          iAcumEsc = iDesde(i)
        End If
      Next
      'si el iDesde(5)<>0 y dPrecio(1)=0 corre un lugar hacia abajo,
      'es el caso de ex combatientes
      If iDesde(5) <> 0 And dPrecio(1) = 0 Then
        For i = 1 To 4
          iDesde(i) = iDesde(i + 1)
          dPrecio(i) = dPrecio(i + 1)
        Next
      End If
      
      'carga en registro
      .Pagina = "T"
      If iSubCat = 1 Then
        .Codigo = CABinarioSP(iCat, Len(.Codigo))
      Else
        .Codigo = CABinarioSP(iCatEsp, Len(.Codigo))
      End If
      .Potencia = CABinarioSP(iDesde(0), Len(.Potencia), 0)
      .Desde_1 = CABinarioSP(iDesde(1), Len(.Desde_1), 0)
      .Desde_2 = CABinarioSP(iDesde(2), Len(.Desde_2), 0)
      .Desde_3 = CABinarioSP(iDesde(3), Len(.Desde_3), 0)
      .Desde_4 = CABinarioSP(iDesde(4), Len(.Desde_4), 0)
      .PrecioPC = CABinarioSP(dPrecio(0), Len(.PrecioPC), DecimalesConceptCoef)
      .Precio_1 = CABinarioSP(dPrecio(1), Len(.Precio_1), DecimalesConceptCoef)
      .Precio_2 = CABinarioSP(dPrecio(2), Len(.Precio_2), DecimalesConceptCoef)
      .Precio_3 = CABinarioSP(dPrecio(3), Len(.Precio_3), DecimalesConceptCoef)
      .Precio_4 = CABinarioSP(dPrecio(4), Len(.Precio_4), DecimalesConceptCoef)
      
      sRegSal = .Pagina & .Codigo & .CuotServ & .Potencia & .PrecioPC & _
                .Desde_1 & .Precio_1 & .Desde_2 & .Precio_2 & _
                .Desde_3 & .Precio_3 & .Desde_4 & .Precio_4
    End With
    lPos = LOF(iTTmp)
    Put #iTTmp, lPos + 1, sRegSal
  Next
  
  GeneraTablaTarifas = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Set rtT = Nothing
  Close #iTTmp
  On Error GoTo 0
  
End Function

Private Function GeneraRegistroCambioMedidor(FechaAnterior As Date, sCarpTmp As String) As Boolean
'Genera un registro de cambio de medidor si el usuario actual tiene cambio
'y lo agrega al archivo temporal, el que se abre y cierra aca.
'El usuario es el que está en el RegIns Activo.-
' EN FechaAnterior: viene la fecha de la lectura anterior, si no hubo cambios, vuelve
'                  la misma, pero si hubo cambios, vuelve la fecha de baja

Dim rtC As Recordset
Dim sQ As String
Dim iCTmp As Integer, sCTmp As String
Dim bOk As Boolean
Dim regCmb As tipoTablaCambioMedidorSP
Dim sRegCmb As String
Dim idx As Integer
Dim lPos As Long

  On Error GoTo errorAca
  GeneraRegistroCambioMedidor = False
  
  
'  idx = nodoCarga.Index
  sQ = " SELECT * FROM [Cambios Medidor] " & _
       " WHERE Conexion = " & CLng(RegIns.RF01.NroConexion)
'       "IN  (SELECT Conexion FROM DatosIns " & _
'            " WHERE LoteCarga = " & infNdX(idx).Lote & _
'            " AND Estado = " & NoCargado_cECx & _
'            " ORDER BY Orden,SubOrden,Conexion) "
  Set rtC = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  If rtC.RecordCount = 0 Then
    'si no hnay cambio sale
    GeneraRegistroCambioMedidor = True
    Set rtC = Nothing
    Exit Function
  End If
  
  'Hay cambios, generar un registro
  'archivo temporal conceptos
  iCTmp = FreeFile
  sCTmp = sCarpTmp & "\CambiosTmp.txt"
  Open sCTmp For Binary As #iCTmp

  rtC.MoveLast
  rtC.MoveFirst
  With regCmb
    .LibresA = " "
    .LibresR = " "
    .Pagina = "B"
    .ActivoCambio.Letra = "    "
    .ActivoCambio.Digitos = Chr(6)
  
    .CmbConex = CABinarioSP(rtC!Conexion, Len(.CmbConex))
    .ActivoCambio.Numero = CABinarioSP(rtC![Medidor Activa], Len(.ActivoCambio.Numero))
    .ActivoCambio.FechaAnterior = CABinarioSP(FechaAnterior, Len(.ActivoCambio.FechaAnterior))
    .ActivoCambio.EstadoAnterior = CABinarioSP(rtC![Estado Alta Activa], Len(.ActivoCambio.EstadoAnterior))
    .ActivoCambio.FechaActual = CABinarioSP(rtC!Fecha, Len(.ActivoCambio.EstadoActual))
    .ActivoCambio.EstadoActual = CABinarioSP(rtC![Estado Baja Activa], Len(.ActivoCambio.EstadoActual))
    .ActivoCambio.FactorMult = CABinarioSP(rtC![Factor Multip Activa], Len(.ActivoCambio.FactorMult))
    .ActivoCambio.TipoConsumo = "  "
    .ActivoCambio.Consumo = CABinarioSP(rtC![Consumo Activa], Len(.ActivoCambio.Consumo))
    FechaAnterior = rtC!Fecha
    
    .ReactivoCambio.Numero = CABinarioSP(rtC![Medidor Reactiva], Len(.ReactivoCambio.Numero))
    '.ReactivoCambio.Letra = "    "
    '.ReactivoCambio.Digitos = Chr(6)
    .ReactivoCambio.FechaAnterior = CABinarioSP(FechaAnterior, Len(.ActivoCambio.FechaAnterior))
    .ReactivoCambio.EstadoAnterior = CABinarioSP(rtC![Estado Alta Reactiva], Len(.ReactivoCambio.EstadoAnterior))
    .ReactivoCambio.FechaActual = CABinarioSP(rtC!Fecha, Len(.ReactivoCambio.EstadoActual))
    .ReactivoCambio.EstadoActual = CABinarioSP(rtC![Estado Baja Reactiva], Len(.ReactivoCambio.EstadoActual))
    '.ReactivoCambio.FactorMult = CABinarioSP(rtC![Factor Multip Reactiva], Len(.ReactivoCambio.FactorMult))
    '.ReactivoCambio.TipoConsumo = "  "
    .ReactivoCambio.Consumo = CABinarioSP(rtC![Consumo Reactiva], Len(.ReactivoCambio.Consumo))
    
      
    sRegCmb = .Pagina & .CmbConex & .ActivoCambio.Numero & .ActivoCambio.Letra & _
              .ActivoCambio.Digitos & .ActivoCambio.FechaAnterior & .ActivoCambio.EstadoAnterior & _
              .ActivoCambio.FechaActual & .ActivoCambio.EstadoActual & .ActivoCambio.FactorMult & _
              .ActivoCambio.TipoConsumo & .ActivoCambio.Consumo & .LibresA & _
              .ReactivoCambio.Numero & .ReactivoCambio.FechaAnterior & .ReactivoCambio.EstadoAnterior & _
              .ReactivoCambio.FechaActual & .ReactivoCambio.EstadoActual & _
              .ReactivoCambio.Consumo & .LibresR
  End With
  lPos = LOF(iCTmp)
  Put #iCTmp, lPos + 1, sRegCmb
  
  GeneraRegistroCambioMedidor = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Set rtC = Nothing
  Close #iCTmp
  On Error GoTo 0

End Function


Private Function GeneraTablaDomicilioPostal(nodoCarga As Node, sCarpTmp As String) As Boolean
'Genera un registro de cambio de medidor si el usuario actual tiene cambio
'y lo agrega al archivo temporal, el que se abre y cierra aca.
'El usuario es el que está en el RegIns Activo.-
' EN FechaAnterior: viene la fecha de la lectura anterior, si no hubo cambios, vuelve
'                  la misma, pero si hubo cambios, vuelve la fecha de baja
Dim rtD As Recordset
Dim rtL As Recordset
Dim sQ As String
Dim iDTmp As Integer, sDTmp As String
Dim iLTmp As Integer, sLTmp As String
Dim bOk As Boolean
Dim regDom As tipoTablaDomicilioPostalSP
Dim regLoc As tipoTablaLocalidadPostalSP
Dim sRegSal As String
Dim idx As Integer
Dim itX As ListItem
Dim lPos As Long

  On Error GoTo errorAca
  GeneraTablaDomicilioPostal = False
  
  idx = nodoCarga.Index
  sQ = " SELECT * FROM [Domicilio Postal] " & _
       " WHERE Conexion" & _
       " IN (SELECT Conexion FROM DatosIns " & _
          " WHERE LoteCarga = " & infNdX(idx).Lote & _
          " AND Estado = " & NoCargado_CECx & _
          " ORDER BY Orden,SubOrden,Conexion) "
  Set rtD = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  If rtD.RecordCount = 0 Then
    'si no hnay cambio sale
    GeneraTablaDomicilioPostal = True
    Set rtD = Nothing
    Exit Function
  End If
  
  'abre la tabla de localidades
  sQ = " SELECT * FROM [Codigo Postal] "
  Set rtL = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  If rtL.RecordCount = 0 Then
    Err.Raise 20015, " Genera Domicilio Postal ", " No se encontró la tabla de Localidades - Código Postal "
  End If
  
'GENERA DOMICILIOS
  'archivo temporal de Domicilios
  iDTmp = FreeFile
  sDTmp = sCarpTmp & "\DomPosTmp.txt"
  Open sDTmp For Binary As #iDTmp

  rtD.MoveFirst
  Lvw.ListItems.Clear
  Do Until rtD.EOF
    With regDom
      'ver si ya está la localidad en la lista
      On Error Resume Next
      Set itX = Lvw.ListItems.Item("s " & Format(rtD!CodigoPostal, "000000"))
      If Err.Number = 0 Then
        'si no da error es porque ya está cargado
        idx = itX.SubItems(1)
      Else
        'si dió error hay que agregar esta localidad
        rtL.FindFirst ("CodigoPostal=" & rtD!CodigoPostal)
        If rtL.NoMatch Then
          'si no encontro la localidad generar un error
          Err.Raise 20021, " Genera Domicilios Postales ", " No existe la localidad para el Código Postal: " & Trim(rtD!CodigoPostal)
        End If
        'encontró, lo agrega a la lista
        idx = Lvw.ListItems.Count + 1
        Set itX = Lvw.ListItems.Add
        itX.Text = rtD!CodigoPostal
        itX.Key = "s " & Format(rtD!CodigoPostal, "000000")
        itX.SubItems(1) = idx
        itX.SubItems(2) = rtL!Localidad
        itX.SubItems(3) = rtL!Provincia
      End If
      .Pagina = "D"
      .Conexion = CABinarioSP(rtD!Conexion, Len(.CodLocalidad))
      .Domicilio = Trim(rtD!Domicilio)
      .CodLocalidad = CABinarioSP(idx, Len(.CodLocalidad))
      sRegSal = .Pagina & .Conexion & .Domicilio & .CodLocalidad
      Lvw.ZOrder
    End With
    lPos = LOF(iDTmp)
    Put #iDTmp, lPos + 1, sRegSal
    rtD.MoveNext
  Loop
  
'GENERA LOCALIDADES
  'archivo temporal de Localidades
  iLTmp = FreeFile
  sLTmp = sCarpTmp & "\LocPosTmp.txt"
  Open sLTmp For Binary As #iLTmp
  
  For idx = 1 To Lvw.ListItems.Count
    With regLoc
      .Pagina = "L"
      .CodLocalidad = CABinarioSP(Lvw.ListItems(idx).SubItems(1), Len(.CodLocalidad))
      .CodigoPostal = CABinarioSP(CLng(Lvw.ListItems(idx).Text), Len(.CodigoPostal))
      .NombreLoc = Trim(Lvw.ListItems(idx).SubItems(2)) & " - " & Trim(Lvw.ListItems(idx).SubItems(3))
      sRegSal = .Pagina & .CodLocalidad & .CodigoPostal & .NombreLoc
    End With
    lPos = LOF(iLTmp)
    Put #iLTmp, lPos + 1, sRegSal
  Next

  GeneraTablaDomicilioPostal = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Set rtD = Nothing
  Set rtL = Nothing
  Close #iDTmp
  Close #iLTmp
  On Error GoTo 0

End Function


Private Function GeneraTablaNovedades(sCarpTmp As String) As Boolean
'Genera tabla de Novedades, Es la misma para todos
Dim rtV As Recordset
Dim sQ As String
Dim iVTmp As Integer, sVTmp As String
Dim sRegSal As String
Dim bOk As Boolean
Dim regNov As tipoTablaNovedadSP
Dim lPos As Long

  On Error GoTo errorAca
  GeneraTablaNovedades = False
  
  'archivo temporal conceptos
  iVTmp = FreeFile
  sVTmp = sCarpTmp & "\NovedadTmp.txt"
  Open sVTmp For Binary As #iVTmp 'Len = LenSectorSP
 
  sQ = " SELECT * FROM Novedades "
  Set rtV = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
  If rtV.RecordCount = 0 Then
    Err.Raise 20020, " Generar Tablas Novedades ", "No hay registros en la Tabla de NOVEDADESo no existe la misma"
  End If
  rtV.MoveFirst
  Do Until rtV.EOF
    With regNov
      .Pagina = "N"
      .Codigo = CABinarioSP(rtV.Fields("Codigo").Value, Len(.Codigo))
      .Texto = Trim(rtV.Fields("Descripcion").Value)
      sRegSal = .Pagina & .Codigo & .Texto
    End With
    lPos = LOF(iVTmp)
    Put #iVTmp, lPos + 1, sRegSal
    rtV.MoveNext
  Loop
  
  GeneraTablaNovedades = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Set rtV = Nothing
  Close #iVTmp
  On Error GoTo 0

End Function

Private Function AgruparTablasDeCarga(nodoCarga As Node, sCarpTemp As String) As Boolean
'junta todas las tablas desde los archivos en la carpeta temporal en
'un solo archivo de salida, genera tambien la tabla Varios (Pagina0)
Dim regPag0 As tipoTablaPagina0SP
Dim i As Integer, idx As Integer
Dim lCP As Long
Dim sc As String
Dim arDes As ArchivosAbiertos
Dim sLin As String
Dim bOk As Boolean
Dim iDesdeStr As Long
Dim iHastaStr As Long

  On Error GoTo errorAca
  AgruparTablasDeCarga = False
  
  idx = nodoCarga.Index
    'armado del archivo de salida juntando todas las partes de los temporales
  'abrir el temporal destino
  arDes.Directorio = sCarpTemp
  arDes.RutNombre = arDes.Directorio & "\Datos.txt"
  arDes.Nro = FreeFile
  'si hubiera uno, lo borra
  Open arDes.RutNombre For Output As #arDes.Nro
  Close #arDes.Nro
  Open arDes.RutNombre For Binary As #arDes.Nro
  
  'se ponen dos sectores vacios al inicio
  sLin = String(LenSectorSP, 0)
  Put #arDes.Nro, 1, sLin
  Put #arDes.Nro, , sLin
  
  bOk = True
  sLin = ""
  With regPag0.Punteros
    '0 tabla de ivas inmediatamente a continuación
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp & "\IvaTmp.txt", arDes.Nro, _
                                         IvasSP_lenRegTabla, iDesdeStr, iHastaStr)
    'la tabla de ivas no tiene punteros debe ser siempre la primera
    
    '1 tipo de medidor, sector vacio
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp, arDes.Nro, 32, iDesdeStr, iHastaStr, "M")
    .TiposMedidor = CABinarioSP(iDesdeStr, Len(.TiposMedidor))
    
    '2 tabla de tarifas
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp & "\TarTmp.txt", arDes.Nro, _
                                           TarifasSP_lenRegTabla, iDesdeStr, iHastaStr)
    .Tarifas = CABinarioSP(iDesdeStr, Len(.Tarifas))
    
    '3 - 4  tabla de Conceptos (Cabeceras y Escalones)
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp & "\CptCabTmp.txt", arDes.Nro, _
                                  ConceptosCabezSP_lenRegTabla, iDesdeStr, iHastaStr)
    .Conceptos = CABinarioSP(iDesdeStr, Len(.Conceptos))
    
    '5 tabla de Domicilios
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp & "\DomPosTmp.txt", arDes.Nro, _
                                 DomicilioPostalSP_lenRegTabla, iDesdeStr, iHastaStr)
    .Domicilio = CABinarioSP(iDesdeStr, Len(.Domicilio))
    
    '6 tabla de Localidades
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp & "\LocPosTmp.txt", arDes.Nro, _
                                  LocalidadPostalSP_lenRegTabla, iDesdeStr, iHastaStr)
    .Localidad = CABinarioSP(iDesdeStr, Len(.Localidad))
    
    '7 tabla de Novedades
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp & "\NovedadTmp.txt", arDes.Nro, _
                                        NovedadSP_lenRegTabla, iDesdeStr, iHastaStr)
    .Novedades = CABinarioSP(iDesdeStr, Len(.Novedades))
    
    '8 tabla de Cambios de Medidores
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp & "\CambiosTmp.txt", arDes.Nro, _
                                   CambioMedidorSP_lenRegTabla, iDesdeStr, iHastaStr)
    .CambioMedidor = CABinarioSP(iDesdeStr, Len(.CambioMedidor))
    
    '9 tabla de Usuarios
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp & "\UsuTmp.txt", arDes.Nro, _
                                        256, iDesdeStr, iHastaStr)
    .UsuPrimero = CABinarioSP(iDesdeStr, Len(.UsuPrimero))  '10
    .UsuActual = .UsuPrimero                                '11
    .UsuSiguiente = .UsuPrimero                             '12
    .UsuUltimo = CABinarioSP(iHastaStr, Len(.UsuUltimo))    '13
    
    '14 altas se agrega un registro en blanco
    If bOk Then bOk = AgregarUnaTablaDeCarga(sCarpTemp, arDes.Nro, 32, iDesdeStr, iHastaStr, "A")
    .Altas = CABinarioSP(iDesdeStr, Len(.Altas))
    '15 posicion del último byte usado es el 0
    .ByteFin = CABinarioSP(0, Len(.ByteFin))
    
    sLin = .TiposMedidor & .Tarifas & .Conceptos & .Domicilio & .Localidad & .Novedades & _
          .CambioMedidor & .UsuPrimero & .UsuUltimo & .UsuSiguiente & .UsuActual & _
          .Altas & .Altas & .ByteFin & .Libres    'altas 2 veces porque es el ultimo sector
  End With
  
'cargar la pagina 0
  With regPag0
    With .Decimales
      .CoefRec2Vto = CABinarioSP(4, Len(.CoefRec2Vto)) ' no se usa mas
      .CoefTarifa = CABinarioSP(4, Len(.CoefTarifa))
      .CptoCoeficiente = CABinarioSP(DecimalesConceptCoef, Len(.CptoCoeficiente))
      .CptoPorcentaje = CABinarioSP(DecimalesConceptPtje, Len(.CptoPorcentaje))
      .MontoImpreso = CABinarioSP(DecimalesMontoImpres, Len(.MontoImpreso))
      .MontoMemoria = CABinarioSP(DecimalesMontoImpres, Len(.MontoMemoria))
      .Libres = " "
      sLin = sLin & .MontoImpreso & .MontoMemoria & .CoefRec2Vto & _
                   .CoefTarifa & .CptoCoeficiente & .CptoPorcentaje & .Libres
    End With
    With .Validacion
      'se leen del archivo ini
      sc = FtPhX.LeerIni("Constantes Varias", "Validacion Lectura Baja", , , 50)
      If IsNumeric(sc) Then .LecturaBaja = CABinarioSP(CLng(sc), Len(.LecturaBaja))
      sc = FtPhX.LeerIni("Constantes Varias", "Validacion Lectura Alta", , , 50)
      If IsNumeric(sc) Then .LecturaAlta = CABinarioSP(CLng(sc), Len(.LecturaAlta))
      sc = FtPhX.LeerIni("Constantes Varias", "Validacion Impresion Baja", , , 110)
      If IsNumeric(sc) Then .ImpresoBaja = CABinarioSP(CLng(sc), Len(.ImpresoBaja))
      sc = FtPhX.LeerIni("Constantes Varias", "Validacion Impresion Alta", , , 110)
      If IsNumeric(sc) Then .ImpresoAlta = CABinarioSP(CLng(sc), Len(.ImpresoAlta))
      sLin = sLin & .LecturaAlta & .LecturaBaja & .ImpresoAlta & .ImpresoBaja
    End With
    With .Estadisticas
      .Impreso = String(Len(.Impreso), 0)
      .Total = CABinarioSP(infNdX(idx).CnxTot, Len(.Total))
      .SinLeer = CABinarioSP(infNdX(idx).CnxTot, Len(.SinLeer))
      .libre = String(Len(.libre), 0)
      sLin = sLin & .Total & .SinLeer & .Impreso & .libre
    End With
    With .Talonarios
      'la parte de nros de facturas no se usa en la carga, solo se usa en el proceso
      'se completa con ceros, y lo demás con los datos que hay
      .PtoVenta = String(Len(.PtoVenta), 0)
      .A = String(Len(.A), 0)
      .B = String(Len(.B), 0)
      .X = String(Len(.X), 0)
      .FechaCarga = CAFechaSP(Now)
      .Lote = infNdX(idx).Lote
      .Libres = " "
      sLin = sLin & .PtoVenta & .X & .A & .B & .C & .Lote & _
                    .Secuencia & .FechaCarga & .Libres
    End With
    With .Varios
      .AcConsors = String(Len(.AcConsors), 0)
      .Anio = CABinarioSP(Format(FtPhX.Parametro("Periodo"), "yy"), Len(.Anio))
      .Periodo = CABinarioSP(Format(FtPhX.Parametro("Periodo"), "mm"), Len(.Periodo))
      .NroRuta = CABinarioSP(infNdX(idx).Ruta, Len(.NroRuta))
      sc = FtPhX.LeerIni("Constantes Varias", "Recargo Segundo Vencimiento", , , 0)
      If IsNumeric(sc) Then .Rec2Vto = CABinarioSP(CLng(sc), Len(.Rec2Vto), 2)
      .EmisLoc = Trim(FtPhX.GetIniSplit("Ruta-Localidad", Format(infNdX(idx).Ruta, "00000"), ";", "Localidad de Emision", "Posadas"))
      'para el nombre de la localidad toma el primer usuario y busca por su código postal
      Get #ArchIns.Nro, 1, RegIns
      lCP = CLng(RegIns.RF03(CpoCalc.CodigoPostal).Valor)
      .NameLoc = Trim(FtPhX.GetIniSplit("Codigo Postal", Format(lCP, "00000"), ";", "Localidad De Suministro"))
      'si no encuentra con el código dado, prueba con sacar la decena de mil
      If Trim(.NameLoc) = "" Then
        lCP = (lCP Mod 10000)
        .NameLoc = Trim(FtPhX.GetIniSplit("Codigo Postal", Format(lCP, "00000"), ";", "Localidad De Suministro", .EmisLoc))
        'si no encontro asi, devuelve la localidad de emision
      End If
      sLin = sLin & .Rec2Vto & .NameLoc & .EmisLoc & .AcConsors & .ResolTari & _
                    .Anio & .Periodo & .NroRuta
    End With
    With .Tomaestados(0)
      'la parte de tomaestados es la ultima parte de la pagina, se completa con ceros
      'directamente antes de cargar al archivo hasta el final del sector
    End With
    
  End With
  
  'se pone esta parte en los dos primeros sectores del archivo de salida
  Put #arDes.Nro, 1, sLin
  Put #arDes.Nro, LenSectorSP + 1, sLin
  
  AgruparTablasDeCarga = bOk
   
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Close #arDes.Nro
  On Error GoTo 0
   
  
End Function


Private Function AgregarUnaTablaDeCarga(sArchivoTabla As String, iNroArchivoDestino As Integer, _
                                        iLenReg As lenRegTabla, iDesde As Long, iHasta As Long, _
                                        Optional sSectorVacio As String = " ") As Boolean
'El archivo destino debe estar abierto, queda abierto
'El archivo de la tabla debe estar cerrado, queda cerrado
'iDesde e iHasta devuelven las posiciones del primer byte del primero y del último sector dentro de
'archivo destino dividido el tamaño del sector.-
'Si sSectorVacio no es un espacio, se ignora el nombre de archivo de origen y en
'destinose coloca un sector vacio con esta letra en la posicion inicial
Dim arOr As ArchivosAbiertos
Dim iLeer As Integer
Dim iCantXSector As Integer
Dim iTotalReg As Integer
Dim lPos As Long
Dim sLin As String

  On Error GoTo errorAca
  
  AgregarUnaTablaDeCarga = False
  
  iDesde = LOF(iNroArchivoDestino) \ LenSectorSP
  
  If sSectorVacio <> " " Then
    sLin = sSectorVacio & String(LenSectorSP - Len(sSectorVacio), 0)
    lPos = LOF(iNroArchivoDestino) + 1    'asegura que vaya al final
    Put #iNroArchivoDestino, lPos, sLin
  Else
    arOr.Nro = FreeFile
    arOr.RutNombre = sArchivoTabla
    Open arOr.RutNombre For Binary Access Read As #arOr.Nro
    iCantXSector = LenSectorSP \ iLenReg
    iTotalReg = LOF(arOr.Nro) \ iLenReg
    If LOF(arOr.Nro) = 0 Then iDesde = 0
    
    Do While Loc(arOr.Nro) < LOF(arOr.Nro)
      iLeer = iCantXSector * iLenReg
      If iLeer < iLenReg Then iLeer = iLenReg
      sLin = Space(iLeer)
      Get #arOr.Nro, , sLin
      If Len(sLin) < LenSectorSP Then sLin = sLin & Space(LenSectorSP - Len(sLin))
      lPos = LOF(iNroArchivoDestino) + 1    'asegura que vaya al final
      Put #iNroArchivoDestino, lPos, sLin
    Loop
  End If
  
  iHasta = IIf(iDesde > 0, LOF(iNroArchivoDestino) \ LenSectorSP - 1, 0)
  
  AgregarUnaTablaDeCarga = True

errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  Close #arOr.Nro
  On Error GoTo 0
   
End Function


Private Function CopiarACarpetaEnvios(nodoCarga As Node, sCarpTemp As String) As Boolean
'Copia el archivo Datos.txt que está en el temporal a la carpeta correspondiente al
'distrito, con el formato de nombre preestablecido, y tambien en la carpeta de
'respaldos.
'Distrito es la Localidad de Emision
Dim idx As Integer
Dim sCarpDest As String, sArDes As String
Dim sDistr As String
  
  CopiarACarpetaEnvios = False
  On Error GoTo errorAca
  'carpeta de envio
  idx = nodoCarga.Index
  sDistr = Trim(FtPhX.GetIniSplit("Ruta-Localidad", Format(infNdX(idx).Ruta, "00000"), ";", "Localidad de Emision", "Error"))
  FtPhX.SetParametro "Distrito", sDistr
  sCarpDest = FtPhX.GetIni("Carpetas", "Dir Cargas Enviar")
  'nombre del archivo
  'EnvYYYYMM-Rut00000-Cnx0000-L0000-Datos.txt
  FtPhX.SetParametro "Ruta", infNdX(idx).Ruta
  FtPhX.SetParametro "CantConex", infNdX(idx).CnxTot
  FtPhX.SetParametro "Lote", infNdX(idx).Lote
  
  'sArDes = "Env" & FtPhX.Parametro("Periodo", "yyyymm")
  'sArDes = sArDes & "-Rut" & Format(infNdX(idx).Ruta, "00000")
  'sArDes = sArDes & "-Cnx" & Format(infNdX(idx).CnxTot, "0000")
  'sArDes = sArDes & "-L" & Format(infNdX(idx).Lote, "0000")
  'sArDes = sArDes & "-Datos.txt"
  sArDes = FtPhX.GetIni("Carpetas", "Fil Cargas Enviar")
  'ruta y archivo
  
  'ver si no hay uno con el mismo nombre (no debería)
  If fso.FileExists(sCarpDest & "\" & sArDes & "-Datos.txt") Then
    Err.Raise 20054, "Capiar a Carpeta Envios", " Ya hay un archivo: " & sArDes & "-Datos.txt" & vbCrLf & _
              " en la carpeta: " & sCarpDest
  End If
  
  'se copia a la carpeta de envio
  CrearSiNoExiste sCarpDest
  fso.CopyFile sCarpTemp & "\Datos.txt", sCarpDest & "\" & sArDes & "-Datos.txt"
  'ver si se copio
  If Not fso.FileExists(sCarpDest & "\" & sArDes & "-Datos.txt") Then
    Err.Raise 20055, "Capiar a Carpeta Envios", " No se pudo copiar el archivo: " & sArDes & "-Datos.txt" & vbCrLf & _
              " en la carpeta: " & sCarpDest
  End If
  
  'se copia un respaldo, agrgandole el distrito antes de "-Datos.txt"
  'Se cambia "Distrito" por "Respaldo" en el nombre de la carpeta de envio
  FtPhX.SetParametro "Distrito", "Respaldo"
  sCarpDest = FtPhX.GetIni("Carpetas", "Dir Cargas Enviar")
  sCarpDest = sCarpDest & "\" & FtPhX.Parametro("Periodo", "yyyymm")
  CrearSiNoExiste sCarpDest
  fso.CopyFile sCarpTemp & "\Datos.txt", sCarpDest & "\" & sArDes & _
                                         "-" & sDistr & "-Datos.txt"
  
  FtPhX.SetParametro "Distrito", sDistr   'para evitar que quede "respaldo" como distrito
  CopiarACarpetaEnvios = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  
  On Error GoTo 0
End Function


Private Function RegistrarLoteCargado(rstCarga As Recordset) As Boolean

  RegistrarLoteCargado = False
  On Error GoTo errorAca

  With rstCarga
    If .RecordCount > 0 Then
      .MoveFirst
      Do Until .EOF
        .Edit
          .Fields("Estado").Value = constEstadoConex.Cargado_CECx
        .Update
        .MoveNext
      Loop
    End If
  End With

  RegistrarLoteCargado = True
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  
  On Error GoTo 0
End Function

Private Function SacaNoAscii(ByVal sCadena As String) As String
'recorre la cadena y remplaza los caracteres no ascii, es decir menores a &H20
'por espacios &H20
Dim i As Integer

  For i = 1 To Len(sCadena)
    If Asc(Mid(sCadena, i, 1)) < 32 Then
      Mid(sCadena, i, 1) = " "
    End If
  Next
  
  SacaNoAscii = sCadena
  
End Function

Private Function SacaSiSoloCerosOEspacios(ByVal sCadena As String) As String
'si cadena tiene solo ceros y/o espacios veuleve solo espacios
Dim i As Integer

  SacaSiSoloCerosOEspacios = sCadena
  For i = 1 To Len(sCadena)
    If Mid(sCadena, i, 1) <> "0" And Mid(sCadena, i, 1) <> " " Then Exit Function
  Next
  'si solo encontro "0" o espacios, vuelve espacios
  SacaSiSoloCerosOEspacios = Space(Len(sCadena))
  
End Function


Private Sub ListaZona()
'solo para sacar la lista de zonas
  Dim sQ As String
  Dim rtZ As Recordset
  Dim i As Integer
  Dim sCp As String
  Dim fTx As TextStream
  Dim sLin As String
  Dim sLoc As String * 23
  Dim sOtr As String * 5
  
  
  Exit Sub
  
  
  On Error GoTo errorAca
  sCp = App.Path & "\Recursos\Ruta-Zona_Localidad.txt"
  Set fTx = fso.CreateTextFile(sCp)
  
  sLin = "RUTA = "
  sOtr = "Letra"
  sLin = sLin & sOtr & ";"
  sOtr = "Zona"
  sLin = sLin & sOtr & ";"
  sLoc = "Localidad de Emision"
  sLin = sLin & sLoc & ";"
  sLoc = "Localidad de Suministro"
  sLin = sLin & sLoc
  
  fTx.WriteLine sLin
  
  For i = 0 To 4000
    sQ = " SELECT * FROM Ruta_Zona " & _
         " WHERE Ruta_Desde <= " & i & _
         " AND Ruta_Hasta >= " & i
    Set rtZ = BaseDatIns.OpenRecordset(sQ, dbOpenDynaset)
    If rtZ.RecordCount > 0 Then
      On Error Resume Next
      sLin = Format(i, "00000") & "= "
      sOtr = " "
      sOtr = rtZ!letra_zona
      sLin = sLin & sOtr & ";"
      sOtr = " "
      sOtr = rtZ!Zona
      sLin = sLin & sOtr & ";"
      sLoc = " "
      sLoc = rtZ!Distrito
      sLin = sLin & sLoc & ";"
      sLoc = " "
      sLoc = rtZ!Localidad
      sLin = sLin & sLoc
      On Error GoTo errorAca
      fTx.WriteLine sLin
    End If
            
  Next
  
errorAca:
  If Err.Number > 0 Then
    If TrataError Then Resume
  End If
  On Error Resume Next
  
  fTx.Close
  rtZ.Close

End Sub

