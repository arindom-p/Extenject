public interface ICarProperties
{
    public int currentCarId { get; protected set; }
    public CarData currentCarData { get; protected set; }
    public float currentCarSpeed { get; protected set; }
}