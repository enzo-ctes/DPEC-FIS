namespace gagFIS_Interfase {
    
    /// <summary>
    /// Constantes usadas para indicar el código de impresión. Este valor indica en que
    /// estadío del proceso de lectura, es decir dentro de la colectora, y cómo regresa
    /// una conexión al ser descargada, se encuentra la conexión. El valor se suma
    /// a la cteCodEstado, para establecer el estado total en que se encuentra una conexión.
    /// <para>La opcion LeidoNoImpreso debe ser temporal entre la lectura y
    /// la impresión</para>
    /// </summary>
    enum cteCodImpres {
        NoLeido = 0,
        LeidoImpreso,
        LeidoNoImpreso,        
        NoImpresoImpresoraDes,
        NoImpresoFueraRango,
        NoImpresoEstadoNegativo,
        NoImpresoErrorDato,
        NoImpresoDomicilioPostal,
        NoImpresoIndicadoDato,
        ImposibleLeer,
        subNegativo,
        ErrorArchDatos,
        ErrorNFact,
        ConexSinConcepFacturar,
        FaltaTitular,
        ConexErrorFacturando = 19,
        ErroEnMemoria,
        ConexPerExcDias,
        ErrorIndeterminado = 99
        
    }

    /// <summary>
    /// (MULTIPLICAR X 100) Estos códigos se usan para indicar en qué estadío del proceso 
    /// se encuentra una conexión, el valor dado aquí se multiplica por 100 y se suma al valor
    /// de cteCodImpres, y entre los dos establecen el valor del estado real de la conexión.
    /// </summary>
    enum cteCodEstado {
        Importado = 0,
        NoCargado = 0,
        EnviadoFIS,   /// Estado que solo se usa en SAP
        TomadoFIS,    /// Estado que solo se usa en SAP
        ParaCargar,
        Cargado,
        Descargado,
        Exportado,
        n1,
        Devuelto,
        RecibidoDeFIS  /// Estado que solo se usa en SAP
    }
    /// <summary>
    /// Constantes de valores usados para indicar el rango dentro del que
    /// está la lectura del medidor
    /// </summary>
    enum cteFueraRango {
        NoProcesado = 0,
        NoHayValor,
        CeroAImpresionBaja,
        ImpresionBajaALecturaBaja,
        LecturaBajaAPromedio,
        PromedioALecturaAlta,
        LecturaAltaAImpresionAlta,
        ImpresionAltaAImpresionConConfirmacion,
        SuperaConConfirmacion
    }

    /// <summary>
    /// Identifica a cada uno de los subtotales
    /// </summary>
    enum cteSubtotales {
        Nada = 0,
        Todo,
        Cuota1,
        Gravado1,
        Gravado2,
        n5, n6, n7, n8, n9,
        n10,
        condIva = 80,
        potencia = 97,
        consuR,
        Consumo
    }

    /// <summary>
    /// Identifica el tipo y el origen del dato. Ya no se usa.
    /// </summary>
    enum cteTipoOrigenDato {
        CantValorEscalon = 0,
        CoefValorEscalon,
        CantDatoCptoDatos,
        CoefDatoCptoDatos
    }
    /// <summary>
    /// Contiene los campos enumerados de la tabla Conexiones para el DOWNLOAD.
    /// </summary>
    enum ImportarConex
    {
        conexionID = 0,
        PersonaID,
        CondIVA,
        Periodo,
        Instalacion,
        FechaCalP,
        usuarioID,
        tituIarID,
        propietarioID,
        DomicSumin,
        BarrioSumin,
        CodPostalSumin,
        CuentaDebito,
        ImpresionCOD,
        Lote,
        Zona,
        Secuencia,
        Remesa,
        Categoria,
        TipoProrrateo,
        ConsumoPromedio,
        PromedioDiario,
        ConsumoResidual,
        CESPnumero,
        CESPvencimiento,
        DocumPago1,
        Vencimiento1,
        DocumPago2,
        VencimientoProx,
        HistoPeriodo01,
        HistoConsumo01,
        HistoPeriodo02,
        HistoConsumo02,
        HistoPeriodo03,
        HistoConsumo03,
        HistoPeriodo04,
        HistoConsumo04,
        HistoPeriodo05,
        HistoConsumo05,
        HistoPeriodo06,
        HistoConsumo06,
        HistoPeriodo07,
        HistoConsumo07,
        HistoPeriodo08,
        HistoConsumo08,
        HistoPeriodo09,
        HistoConsumo09,
        HistoPeriodo10,
        HistoConsumo10,
        HistoPeriodo11,
        HistoConsumo11
    }

    enum CantColectoras
    {
        CAPITAL = 20,
        GOYA = 4,
        LIBRES = 4,
        MERCEDES = 4,
        PERUGORRIA = 1,
        ITUZAINGO = 2,
        VIRASORO =3,
        CURUZU = 4,
        SAUCE = 1,
        SANTOTOME = 3,
        LACRUZ = 2,
        BELLAVISTA = 3,
        SANTALUCIA = 2,
        CAACATI = 4,
        SANTAROSA = 2,
        MONTECASEROS = 3,
        MOCORETA = 1,
        SALADAS = 2,
        SANROQUE = 1,
        ESQUINA = 2

    }

    /// <summary>
    /// Valor regido por el concepto octal.
    /// Es del tipo octal, y se usan los cuatro bits, aunque el valor no puede ser nunca superior a 9, pero como la condición de NO Leído invalida cualquier otra posibilidad, se establece el bit 3 (más significativo) con valor ‘1’ cuando está leído sin ninguna otra condición. Una vez que se leyó, este bit pasa a ‘1’, pero como cualquiera de las otras condiciones solo puede darse si fue leído, en cuando se coloca en ‘1’ cualquiera de ellos, se resetea el bit 3
    ///•	Bit 0 = 1: Impreso
    ///•	Bit 1 = 1: Ingresó Corrección de Estado.
    ///•	Bit 2 = 1: Medidor con Tele Lectura.
    ///•	Bit 3 = 1:
    ///             o   8 = Leído sin otro condición
    ///             o   9 = Apagado.

    /// </summary>
    enum ImpCntStatus
    {
        NoLeido = 0,
        LeidoConCorrecionEstado, //bit 0 = 1
        LeidoConTeleLectura, // bit 0 y 3 = 1
        LeidoConCorrecionEstadoyTeleLectura, // bit 2 y 1 = 1
        LeidoImpreso, // bit 0 y 3 = 1
        LeidoImpresoConCorreccion, // bit 1 y 3 = 1
        LeidoImpresoConTeleLectura, // bit 1 y 3 = 1
        LeidoImpresoConTeleLecturaCorrecionEstado, // bit 1 y 3 = 1
        Leido, // bit 1 y 3 = 1
        Apagado,
        //LeidoImpresoCorreccionEstado,
        //LeidoNOImpresoErrorGrafica,
        //NoPuedeDarse5,
        //LeidoNoImpresoCorreccionEstadoErrorGrafica,
        //NoPuedeDarse7,
      

    }

    enum ImpCntRango
    {
        Indeterminado = 0,
        DebajoMinimoImprimible,
        ConsumoMuyBajo,
        ImpresionBajaConConfirmacion,
        ImpresionBaja,
        DentroRangoLectura,
        ImpresionAlta,
        ImpresionAltaConConfirmacion,        
        ConsumoMuyAlto,
        LecturaImposible
    }

    enum ImpCntPrinter
    {
        SinNovedad = 0,
        ImpresoraDeshabilitada,
        ImpresoraApagada,
        ImpresoraNoVinculada,
        ErrorDeImpresora,
        ErrorComunicacionConImpresora,
        ErrorAlGererarGraficaDeFactura = 7,
        MarcadoParaImpEnLote,
        ImpresoEnLote ///solo vale si ImpCntStatus = 1
    }

    enum ImpCntNoved
    {
        SinNovedad = 0,
        OrdenativosNoImprimibles,
        OrdenativoParaEstimacion,
        TarifasNoImprimibles
    }

    enum ImpCntPeri
    {
        Normal = 0,
        ExcedeLimiteFacturacion,
        ExcedeLimiteLectura,
    }

    enum ImpCntIndDat
    {
        NoHayIndicacion = 0,
        Indefinido,
        DiferenciaDeDomicilioPostalYSuministro,
        DiferenciaDeLocalidadPostalYSuministro,
    }

    enum ImpCntPosFac
    {
        Normal = 0,
        ExcedeConsumoOErrorFormato,
        ExcedeImporteOErrorFormato,
        ImporteNegativoEnAlgunTalon,
        ExcedeDiasPeriodoFacturacion,
        PeriodoNoCorresponde,
        VencimientosMal,
        ExcesoDeRenglones
    }

    enum ImpCntWS
    {
        SinNovedad = 0,
        ErrorInformadoPorSAP,
        NoContestaWebServer,
        NoHayCoberturaRed,
        NoHayRegistroImpresorFactura,
        TryCatchEnParteWS
    }

    enum ImpCntCantCompImpr
    {
        SinNovedad = 0
       
    }

   
}