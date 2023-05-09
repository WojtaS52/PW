namespace Dane
{
    public class DaneApi : DaneAbstractApi
    {
        //ustawiamy tutaj parametry
        public override int WysokoscPlanszy { get; } = 200;

        public override int SzerokoscPlanszy { get; } = 350;

        public override int minSrednicaKuli { get; } = 20;

        public override int maxSrednicaKuli { get; } = 50;

        public override float predkosc { get; } = 30f;

    }
}