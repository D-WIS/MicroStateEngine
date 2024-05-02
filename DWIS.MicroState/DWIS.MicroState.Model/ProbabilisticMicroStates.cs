using OSDC.DotnetLibraries.Drilling.DrillingProperties;
using System.Reflection;
using DWIS.API.DTO;
using DWIS.Client.ReferenceImplementation;
using System.ComponentModel.Design;

namespace DWIS.MicroState.Model
{
    public class ProbabilisticMicroStates
    {
        private static string prefix_ = "DWIS:MicroState:ProbabilisticMicroStates:";
        private static string companyName_ = "DWIS";

        /// <summary>
        /// the time stamp in UTC when the state has been updated
        /// </summary>
        public DateTime TimeStampUTC { get; set; }

        //AxialVelocityTopOfString = 0,
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty AxialVelocityTopOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableAxialVelocityTopOfString, // 1
        [SemanticBernoulliVariable("")]
        public BernoulliDrillingProperty StableAxialVelocityTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    RotationalVelocityTopOfString, // 2
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty RotationalVelocityTopOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableRotationalVelocityTopOfString, // 3
        public BernoulliDrillingProperty StableRotationalVelocityTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    FlowAtTopOfString, // 4
        public BernoulliDrillingProperty FlowAtTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    StableFlowAtTopOfString, // 5
        public BernoulliDrillingProperty StableFlowAtTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    SlipState, // 6
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty SlipState { get; set; } = new CategoricalDrillingProperty(3);
        //    StableTensionTopOfString, // 7
        public BernoulliDrillingProperty StableTensionTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    PressureTopOfString, // 8
        public BernoulliDrillingProperty PressureTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    StablePressureTopOfString, // 9
        public BernoulliDrillingProperty StablePressureTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    TorqueTopOfString, // 10
        public BernoulliDrillingProperty TorqueTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    StableTorqueTopOfString, // 11
        public BernoulliDrillingProperty StableTorqueTopOfString { get; set; } = new BernoulliDrillingProperty();
        //    FlowAtAnnulusOutlet, // 12
        public BernoulliDrillingProperty FlowAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    StableFlowAtAnnulusOutlet, // 13
        public BernoulliDrillingProperty StableFlowAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    CuttingsReturnAtAnnulusOutlet, // 14
        public BernoulliDrillingProperty CuttingsReturnAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    OnBottomBottomOfString, // 15
        public BernoulliDrillingProperty OnBottomBottomOfString { get; set; } = new BernoulliDrillingProperty();
        //    StableBottomOfStringRockForce, // 16
        public BernoulliDrillingProperty StableBottomOfStringRockForce { get; set; } = new BernoulliDrillingProperty();
        //    OnBottomHoleOpener, // 17
        public BernoulliDrillingProperty OnBottomHoleOpener { get; set; } = new BernoulliDrillingProperty();
        //    RotationalVelocityBottomOfString, // 18
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty RotationalVelocityBottomOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableRotationalVelocityBottomOfString, // 19
        public BernoulliDrillingProperty StableRotationalVelocityBottomOfString { get; set; } = new BernoulliDrillingProperty();
        //    Drilling, // 20
        public BernoulliDrillingProperty Drilling { get; set; } = new BernoulliDrillingProperty();
        //    HoleOpening, // 21
        public BernoulliDrillingProperty HoleOpening { get; set; } = new BernoulliDrillingProperty();
        //    AxialVelocityBottomOfString, //22
        public CategoricalDrillingProperty AxialVelocityBottomOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableAxialVelocityBottomOfString, // 23
        public BernoulliDrillingProperty StableAxialVelocityBottomOfString { get; set; } = new BernoulliDrillingProperty();
        //    FlowBottomOfString, // 24
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty FlowBottomOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    StableFlowBottomOfString, // 25
        public BernoulliDrillingProperty StableFlowBottomOfString { get; set; } = new BernoulliDrillingProperty();
        //    FlowHoleOpener, // 26
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty FlowHoleOpener { get; set; } = new CategoricalDrillingProperty(3);
        //    StableFlowHoleOpener, // 27
        public BernoulliDrillingProperty StableFlowHoleOpener { get; set; } = new BernoulliDrillingProperty();
        //    LedgeKeySeat, // 28
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty LedgeKeySeat { get; set; } = new CategoricalDrillingProperty(3);
        //    CuttingsBed, // 29
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty CuttingsBed { get; set; } = new CategoricalDrillingProperty(3);
        //    DifferentialSticking, // 30
        public BernoulliDrillingProperty DifferentialSticking { get; set; } = new BernoulliDrillingProperty();
        //    TwistOffBackOff, // 31
        public BernoulliDrillingProperty TwistOffBackOff { get; set; } = new BernoulliDrillingProperty();
        //    WellIntegrity, // 32
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty WellIntegrity { get; set; } = new CategoricalDrillingProperty(3);
        //    FormationFluidAtAnnulusOutlet, // 33
        public BernoulliDrillingProperty FormationFluidAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    FormationCollapse, // 34
        public BernoulliDrillingProperty FormationCollapse { get; set; } = new BernoulliDrillingProperty();
        //    CavingsAtAnnulusOutlet, // 35
        public BernoulliDrillingProperty CavingsAtAnnulusOutlet { get; set; } = new BernoulliDrillingProperty();
        //    PipeWashout, // 36
        public BernoulliDrillingProperty PipeWashout { get; set; } = new BernoulliDrillingProperty();
        //    WhirlBottomOfString, // 37
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty WhirlBottomOfString { get; set; } = new CategoricalDrillingProperty(3);
        //    WhirlHoleOpener, // 38
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty WhirlHoleOpener { get; set; } = new CategoricalDrillingProperty(3);
        //    FloatSub, // 39
        public BernoulliDrillingProperty FloatSub { get; set; } = new BernoulliDrillingProperty();
        //    UnderReamer, // 40
        public BernoulliDrillingProperty UnderReamer { get; set; } = new BernoulliDrillingProperty();
        //    CirculationSub, // 41
        public BernoulliDrillingProperty CirculationSub { get; set; } = new BernoulliDrillingProperty();
        //    PortedFloat, // 42
        public BernoulliDrillingProperty PortedFloat { get; set; } = new BernoulliDrillingProperty();
        //    Whipstock, // 43
        public BernoulliDrillingProperty Whipstock { get; set; } = new BernoulliDrillingProperty();
        //    Plug, // 44
        public BernoulliDrillingProperty Plug { get; set; } = new BernoulliDrillingProperty();
        //    Liner, // 45
        public BernoulliDrillingProperty Liner { get; set; } = new BernoulliDrillingProperty();
        //    BoosterPumping, // 46
        public BernoulliDrillingProperty BoosterPumping { get; set; } = new BernoulliDrillingProperty();
        //    StableBoosterPumping, // 47
        public BernoulliDrillingProperty StableBoosterPumping { get; set; } = new BernoulliDrillingProperty();
        //    BackPressurePumping, // 48
        public BernoulliDrillingProperty BackPressurePumping { get; set; } = new BernoulliDrillingProperty();
        //    StableBackPressurePumping, // 49
        public BernoulliDrillingProperty StableBackPressurePumping { get; set; } = new BernoulliDrillingProperty();
        //    MPDChokeOpening, // 50
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty MPDChokeOpening { get; set; } = new CategoricalDrillingProperty(3);
        //    RCDSealing, // 51
        public BernoulliDrillingProperty RCDSealing { get; set; } = new BernoulliDrillingProperty();
        //    IsolationSeal, // 52
        public BernoulliDrillingProperty IsolationSeal { get; set; } = new BernoulliDrillingProperty();
        //    IsolationSealPressureBalance, // 53
        public BernoulliDrillingProperty IsolationSealPressureBalance { get; set; } = new BernoulliDrillingProperty();
        //    BearingAssemblyLatched, // 54
        public BernoulliDrillingProperty BearingAssemblyLatched { get; set; } = new BernoulliDrillingProperty();
        //    ScreenMPDChokePlugged, // 55
        public BernoulliDrillingProperty ScreenMPDChokePlugged { get; set; } = new BernoulliDrillingProperty();
        //    MainFlowPathStable, // 56
        public BernoulliDrillingProperty MainFlowPathStable { get; set; } = new BernoulliDrillingProperty();
        //    AlternateFlowPathStable, // 57
        public BernoulliDrillingProperty AlternateFlowPathStable { get; set; } = new BernoulliDrillingProperty();
        //    FillPumpDGD, // 58
        public BernoulliDrillingProperty FillPumpDGD { get; set; } = new BernoulliDrillingProperty();
        //    LiftPumpDGD, // 59
        public BernoulliDrillingProperty LiftPumpDGD { get; set; } = new BernoulliDrillingProperty();
        //    StableFillPumpDGD, // 60
        public BernoulliDrillingProperty StableFillPumpDGD { get; set; } = new BernoulliDrillingProperty();
        //    StableLiftPumpDGD, // 61
        public BernoulliDrillingProperty StableLiftPumpDGD { get; set; } = new BernoulliDrillingProperty();
        //    FormationChange, // 62
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty FormationChange { get; set; } = new CategoricalDrillingProperty(3);
        //    InsideHardStringer, // 63
        public BernoulliDrillingProperty InsideHardStringer { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint1AtLowestDrillHeight, // 64
        public BernoulliDrillingProperty ToolJoint1AtLowestDrillHeight { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint1AtStickUpHeight, // 65
        public BernoulliDrillingProperty ToolJoint1AtStickUpHeight { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint2AtStickUpHeight, // 66
        public BernoulliDrillingProperty ToolJoint2AtStickUpHeight { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint3AtStickUpHeight, // 67
        public BernoulliDrillingProperty ToolJoint3AtStickUpHeight { get; set; } = new BernoulliDrillingProperty();
        //    ToolJoint4AtStickUpHeight, // 68
        public BernoulliDrillingProperty ToolJoint4AtStickUpHeight { get; set; } = new BernoulliDrillingProperty();
        //    HeaveCompensation, // 69
        [SemanticCategoricalVariable("", 3)]
        public CategoricalDrillingProperty HeaveCompensation { get; set; } = new CategoricalDrillingProperty(3);
        //    LastStandToBottomHole, // 70
        public BernoulliDrillingProperty LastStandToBottomHole { get; set; } = new BernoulliDrillingProperty();
        //    WhirlInDrillString, // 71
        public BernoulliDrillingProperty WhirlInDrillString { get; set; } = new BernoulliDrillingProperty();
        //    HFTO, // 72
        public BernoulliDrillingProperty HFTO { get; set; } = new BernoulliDrillingProperty();
        //    AxialOscillations, // 73
        public BernoulliDrillingProperty AxialOscillations { get; set; } = new BernoulliDrillingProperty();
        //    TorsionalOscillations, // 74
        public BernoulliDrillingProperty TorsionalOscillations { get; set; } = new BernoulliDrillingProperty();
        //    LateralShocksInBHA, // 75
        public BernoulliDrillingProperty LateralShocksInBHA { get; set; } = new BernoulliDrillingProperty();
        //    LateralShokcsInDrillString // 76
        public BernoulliDrillingProperty LateralShokcsInDrillString { get; set; } = new BernoulliDrillingProperty();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(ProbabilisticMicroStates other)
        {
            if (other == null)
            {
                return false;
            }
            else
            {
                bool equal = true;
                Type type = GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property != null && property.PropertyType == typeof(DrillingProperty))
                    {
                        object? val1 = property.GetValue(this);
                        object? val2 = property.GetValue(other);
                        if (val1 is not null and DrillingProperty drillProp1 && val2 is not null and DrillingProperty drillProp2)
                        { 
                            equal &= drillProp1.Equals(drillProp2);
                        }
                        else
                        {
                            equal &= val1 == null && val2 == null;
                        }
                    }
                }
                return equal;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dest"></param>
        public void CopyTo(ProbabilisticMicroStates? dest)
        {
            if (dest != null)
            {
                Type type = GetType();
                PropertyInfo[] properties = type.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    if (property != null && property.PropertyType == typeof(DrillingProperty))
                    {
                        object? val1 = property.GetValue(this);
                        object? val2 = property.GetValue(dest);
                        if (val1 is not null and DrillingProperty drillProp1 && val2 is not null and DrillingProperty drillProp2)
                        {
                            drillProp1.CopyTo(drillProp2);
                        }
                    }
                }
            }
        }

        public bool RegisterToDDHub(IOPCUADWISClient? DWISClient, Dictionary<string, QueryResult>? placeHolders)
        {
            if (DWISClient != null && placeHolders != null)
            {
                Type type = GetType();
                Assembly assembly = type.Assembly;
                PropertyInfo[] properties = type.GetProperties();
                
                bool ok = true;
                foreach (PropertyInfo property in properties)
                {
                    if (property != null && property.PropertyType == typeof(DiscreteDrillingProperty))
                    {
                        string propName = property.Name;
                        if (!string.IsNullOrEmpty(propName))
                        {
                            string manifestName = type.FullName + "_" + propName;
                            ManifestFile? manifestFile = GeneratorSparQLManifestFile.GetManifestFile(assembly, type.FullName, propName, manifestName, companyName_, prefix_);
                            Dictionary<string, QuerySpecification>? queries = GeneratorSparQLManifestFile.GetSparQLQueries(assembly, type.FullName, propName);
                            if (queries != null && queries.Count > 0 && manifestFile != null)
                            {
                                QueryResult? res = null;
                                foreach (var kvp in queries)
                                {
                                    if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                                    {
                                        var result = DWISClient.GetQueryResult(kvp.Value.SparQL);
                                        if (result != null && result.Results != null && result.Results.Count > 0)
                                        {
                                            res = result;
                                            break;
                                        }
                                    }
                                }
                                // if we couldn't find any answer then the manifest must be injected
                                if (res == null)
                                {
                                    var r = DWISClient.Inject(manifestFile);
                                    if (r != null && r.Success )
                                    {
                                        res = null;
                                        foreach (var kvp in queries)
                                        {
                                            if (kvp.Value != null && !string.IsNullOrEmpty(kvp.Value.SparQL))
                                            {
                                                var result = DWISClient.GetQueryResult(kvp.Value.SparQL);
                                                if (result != null && result.Results != null && result.Results.Count > 0)
                                                {
                                                    res = result;
                                                    break;
                                                }
                                            }
                                        }
                                        if (res != null)
                                        {
                                            placeHolders.Add(propName, res);
                                        }
                                        else
                                        {
                                            ok = false;
                                        }
                                    }
                                }
                                else
                                {
                                    // a manifest has already been injected.
                                    placeHolders.Add(propName, res);
                                }
                            }
                        }
                    }
                }
                return ok;
            }
            else
            {
                return false;
            }
        }

        public bool SendToDDHub(IOPCUADWISClient? DWISClient, Dictionary<string, QueryResult>? placeHolders)
        {
            bool ok = false;
            if (DWISClient != null && placeHolders != null)
            {
                Type type = GetType();
                PropertyInfo[] properties = type.GetProperties();
                ok = true;
                foreach (PropertyInfo property in properties)
                {
                    bool ok1 = false;
                    if (property != null && property.PropertyType == typeof(DiscreteDrillingProperty))
                    {
                        string propName = property.Name;
                        if (!string.IsNullOrEmpty(propName) && placeHolders.ContainsKey(propName))
                        {
                            QueryResult? queryResult = placeHolders[propName];
                            if (queryResult != null && queryResult.Count > 0 && queryResult[0].Count > 0)
                            {
                                object? propValue = property.GetValue(this);
                                if (propValue is not null and DiscreteDrillingProperty discretePropValue)
                                {
                                    uint? numberOfStates = discretePropValue.NumberOfStates;
                                    double[]? probabilities = discretePropValue.Probabilities;
                                    NodeIdentifier id = queryResult[0][0];
                                    if (numberOfStates != null && probabilities != null && probabilities.Length >= numberOfStates &&
                                        id != null && !string.IsNullOrEmpty(id.ID) && !string.IsNullOrEmpty(id.NameSpace))
                                    {
                                        // OPC-UA code to set the value at the node id = ID
                                        (string nameSpace, string id, object value, DateTime sourceTimestamp)[] outputs = new (string nameSpace, string id, object value, DateTime sourceTimestamp)[1];
                                        outputs[0].nameSpace = id.NameSpace;
                                        outputs[0].id = id.ID;
                                        outputs[0].value = probabilities;
                                        outputs[0].sourceTimestamp = DateTime.UtcNow;
                                        DWISClient.UpdateAnyVariables(outputs);
                                        ok1 = true;
                                    }
                                }
                            }
                        }
                    }
                    ok &= ok1;
                }
            }
            return ok;
        }
    }
}
