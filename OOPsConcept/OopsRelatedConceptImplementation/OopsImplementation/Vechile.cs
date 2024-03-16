namespace OopsRelatedConceptImplementation.OopsImplementation;

abstract class Vechile
{
    public string Make { get; set; }
    public string Model { get; set; }
    public string EngineStatus { get; private set; } ="Off";
    public Vechile(string make, string model) => (Make, Model) = (make, model);
    protected virtual void BeforeEngineStart() { }
    protected virtual void AfterEngineStart() { }
    public void StartEngine()
    {
        if (EngineStatus != "Off") return;
        BeforeEngineStart();
        EngineStatus = "Idle";
        AfterEngineStart();
    }
}

class Car : Vechile
{
    public string ScreenContext { get; private set; } = "Off";
    public Car(string make, string model) : base(make, model)
    {
    }
    protected override void AfterEngineStart()
    {
        base.AfterEngineStart();
        ScreenContext = "BasicInfo";
    }
}
