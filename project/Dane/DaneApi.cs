using Dane.API;

namespace Dane
{
    public class DaneApi : DaneAbstractApi
    {
        //ustawiamy tutaj parametry
        public override int WysokoscPlanszy { get; } = 244;

        public override int SzerokoscPlanszy { get; } = 477;

        public override int minSrednicaKuli { get; } = 16;

        public override int maxSrednicaKuli { get; } = 38;

        public override float predkosc { get; } = 20f;

    }
}