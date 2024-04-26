namespace Source.LevelLoader
{
    public interface IFader
    {
        public float Fade { get; }

        public void SetFade(float fade);
    }
}
