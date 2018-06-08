namespace Components.Healthable
{

  public interface IHealthableData
  {
      float BaseHealthCapacity { get; }
      float BaseHealthGen { get; }
      float GainHealthCapacity { get; }
      float GainHealthGen { get; }
    }

}