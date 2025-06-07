namespace CarReservations.Razor.Pages.Shared
{
    public class CarVm
    {
        public Guid Id { get; set; }
        public string Brand { get; set; } = "";
        public string Model { get; set; } = "";
        public int Year { get; set; }
    }
}
