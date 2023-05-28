using Dane.API;

namespace Dane
{
    internal class DaneApi : DaneAbstractApi
    {
        //ustawiamy tutaj parametry
        public override int WysokoscPlanszy => 244;

        public override int SzerokoscPlanszy => 477;

        public override int minSrednicaKuli => 20;

        public override int maxSrednicaKuli =>   50;

        public override float predkosc => 30f;

    }
}