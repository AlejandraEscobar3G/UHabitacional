namespace UHabitacionalAPI.Remote.DTOs
{
    public class DragonBallCharactersResponse
    {
        public List<DragonBallCharacterResponse> Items { get; set; } = new();
        public DragonBallMeta Meta { get; set; } = new();
        public DragonBallLinks Links { get; set; } = new();
    }
}
