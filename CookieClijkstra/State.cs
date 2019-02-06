using System;

namespace CookieClijkstra
{
    public struct State
    {
        private const float cost_base = 1.15f;

        private const float cps_mouse = 10;

        private const float cps_cursor = 0.1f;
        private const float cps_grandma = 1;
        private const float cps_farm = 8;
        private const float cps_mine = 47;
        private const float cps_factory = 260;

        private const float cost_cursor = 15;
        private const float cost_grandma = 100;
        private const float cost_farm = 1100;
        private const float cost_mine = 12000;
        private const float cost_factory = 130000;

        private const byte gate_reinforced_index_finger = 1;
        private const byte gate_carpal_tunnel_prevention_cream = 1;
        private const byte gate_ambidextrous = 10;
        private const byte gate_thousand_fingers = 25;

        private const byte gate_forwards_from_grandma = 1;
        private const byte gate_steel_plated_rolling_pins = 5;
        private const byte gate_lubricated_dentures = 25;

        private const byte gate_cheap_hoes = 1;
        private const byte gate_fertilizer = 5;
        private const byte gate_cookie_trees = 25;

        private const byte gate_sugar_gas = 1;
        private const byte gate_megadrill = 5;

        public const int COST_REINFORCED_INDEX_FINGER = 100;
        public const int COST_CARPAL_TUNNEL_PREVENTION_CREAM = 500;
        public const int COST_AMBIDEXTROUS = 10000;
        public const int COST_THOUSAND_FINGERS = 100000;

        public const int COST_FORWARDS_FROM_GRANDMA = 1000;
        public const int COST_STEEL_PLATED_ROLLING_PINS = 5000;
        public const int COST_LUBRICATED_DENTURES = 50000;

        public const int COST_CHEAP_HOES = 11000;
        public const int COST_FERTILIZER = 55000;
        public const int COST_COOKIE_TREES = 550000;

        public const int COST_SUGAR_GAS = 120000;
        public const int COST_MEGADRILL = 600000;

        public byte Cursors;
        public int CursorCost => Cost(cost_cursor, this.Cursors);

        public bool ReinforcedIndexFinger;
        public bool ReinforcedIndexFingerAvailable => !(this.ReinforcedIndexFinger || this.Cursors < gate_reinforced_index_finger);
        public bool CarpalTunnelPreventionCream;
        public bool CarpalTunnelPreventionCreamAvailable => !(this.CarpalTunnelPreventionCream || this.Cursors < gate_carpal_tunnel_prevention_cream);
        public bool Ambidextrous;
        public bool AmbidextrousAvailable => !(this.Ambidextrous || this.Cursors < gate_ambidextrous);
        public bool ThousandFingers;
        public bool ThousandFingersAvailable => !(this.ThousandFingers || this.Cursors < gate_thousand_fingers);

        public byte Grandmas;
        public int GrandmaCost => Cost(cost_grandma, this.Grandmas);

        public bool ForwardsFromGrandma;
        public bool ForwardsFromGrandmaAvailable => !(this.ForwardsFromGrandma || this.Grandmas < gate_forwards_from_grandma);
        public bool SteelPlatedRollingPins;
        public bool SteelPlatedRollingPinsAvailable => !(this.SteelPlatedRollingPins || this.Grandmas < gate_steel_plated_rolling_pins);
        public bool LubricatedDentures;
        public bool LubricatedDenturesAvailable => !(this.LubricatedDentures || this.Grandmas < gate_lubricated_dentures);

        public byte Farms;
        public int FarmCost => Cost(cost_farm, this.Farms);

        public bool CheapHoes;
        public bool CheapHoesAvailable => !(this.CheapHoes || this.Farms < gate_cheap_hoes);
        public bool Fertilizer;
        public bool FertilizerAvailable => !(this.Fertilizer || this.Farms < gate_fertilizer);
        public bool CookieTrees;
        public bool CookieTreesAvailable => !(this.CookieTrees || this.Farms < gate_cookie_trees);

        public byte Mines;
        public int MineCost => Cost(cost_mine, this.Mines);

        public bool SugarGas;
        public bool SugarGasAvailable => !(this.SugarGas || this.Mines < gate_sugar_gas);
        public bool Megadrill;
        public bool MegadrillAvailable => !(this.Megadrill || this.Mines < gate_megadrill);

        public byte Factories;
        public int FactoryCost => Cost(cost_factory, this.Factories);

        public float CookiesPerSecond
        {
            get
            {
                float mouseCps = cps_mouse;

                float cursorCps = cps_cursor * this.Cursors;
                float grandmaCps = cps_grandma * this.Grandmas;
                float farmCps = cps_farm * this.Farms;
                float mineCps = cps_mine * this.Mines;
                float factoryCps = cps_factory * this.Factories;

                if (this.ReinforcedIndexFinger)
                {
                    mouseCps *= 2;
                    cursorCps *= 2;
                }
                if (this.CarpalTunnelPreventionCream)
                {
                    mouseCps *= 2;
                    cursorCps *= 2;
                }
                if (this.Ambidextrous)
                {
                    mouseCps *= 2;
                    cursorCps *= 2;
                }
                if (this.ThousandFingers)
                {
                    float addition = (this.Grandmas + this.Farms + this.Mines + this.Factories) * 0.1f;

                    mouseCps += addition;
                    cursorCps += addition;
                }

                if (this.ForwardsFromGrandma)
                {
                    grandmaCps *= 2;
                }
                if (this.SteelPlatedRollingPins)
                {
                    grandmaCps *= 2;
                }
                if (this.LubricatedDentures)
                {
                    grandmaCps *= 2;
                }

                if (this.CheapHoes)
                {
                    farmCps *= 2;
                }
                if (this.Fertilizer)
                {
                    farmCps *= 2;
                }
                if (this.CookieTrees)
                {
                    farmCps *= 2;
                }

                if (this.SugarGas)
                {
                    mineCps *= 2;
                }
                if (this.Megadrill)
                {
                    mineCps *= 2;
                }

                return mouseCps + cursorCps + grandmaCps + farmCps + mineCps + factoryCps;
            }
        }

        public int CookiesAlreadySpent
        {
            get
            {
                int mouseCost = Sum(cost_cursor, this.Cursors);
                int grandmaCost = Sum(cost_grandma, this.Grandmas);
                int farmCost = Sum(cost_farm, this.Farms);
                int mineCost = Sum(cost_mine, this.Mines);
                int factoryCost = Sum(cost_factory, this.Factories);

                int totalCookies = mouseCost + grandmaCost + farmCost + mineCost + factoryCost;

                totalCookies += this.ReinforcedIndexFinger ? COST_REINFORCED_INDEX_FINGER : 0;
                totalCookies += this.CarpalTunnelPreventionCream ? COST_CARPAL_TUNNEL_PREVENTION_CREAM : 0;
                totalCookies += this.Ambidextrous ? COST_AMBIDEXTROUS : 0;
                totalCookies += this.ThousandFingers ? COST_THOUSAND_FINGERS : 0;

                totalCookies += this.ForwardsFromGrandma ? COST_FORWARDS_FROM_GRANDMA : 0;
                totalCookies += this.SteelPlatedRollingPins ? COST_STEEL_PLATED_ROLLING_PINS : 0;
                totalCookies += this.LubricatedDentures ? COST_LUBRICATED_DENTURES : 0;

                totalCookies += this.CheapHoes ? COST_CHEAP_HOES : 0;
                totalCookies += this.Fertilizer ? COST_FERTILIZER : 0;
                totalCookies += this.CookieTrees ? COST_COOKIE_TREES : 0;

                totalCookies += this.SugarGas ? COST_SUGAR_GAS : 0;
                totalCookies += this.Megadrill ? COST_MEGADRILL : 0;

                return totalCookies;
            }
        }

        private static int Cost(float baseCost, int ownedCount) => (int)Math.Ceiling(baseCost * Math.Pow(cost_base, ownedCount));

        private static int Sum(float baseCost, int count)
        {
            int sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Cost(baseCost, i);
            }

            return sum;
        }

        public State(State other)
        {
            this.Cursors = other.Cursors;
            this.ReinforcedIndexFinger = other.ReinforcedIndexFinger;
            this.CarpalTunnelPreventionCream = other.CarpalTunnelPreventionCream;
            this.Ambidextrous = other.Ambidextrous;
            this.ThousandFingers = other.ThousandFingers;

            this.Grandmas = other.Grandmas;
            this.ForwardsFromGrandma = other.ForwardsFromGrandma;
            this.SteelPlatedRollingPins = other.SteelPlatedRollingPins;
            this.LubricatedDentures = other.LubricatedDentures;

            this.Farms = other.Farms;
            this.CheapHoes = other.CheapHoes;
            this.Fertilizer = other.Fertilizer;
            this.CookieTrees = other.CookieTrees;

            this.Mines = other.Mines;
            this.SugarGas = other.SugarGas;
            this.Megadrill = other.Megadrill;

            this.Factories = other.Factories;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + this.Cursors.GetHashCode();
                hash = (hash * 23) + this.ReinforcedIndexFinger.GetHashCode();
                hash = (hash * 23) + this.CarpalTunnelPreventionCream.GetHashCode();
                hash = (hash * 23) + this.Ambidextrous.GetHashCode();
                hash = (hash * 23) + this.ThousandFingers.GetHashCode();
                hash = (hash * 23) + this.Grandmas.GetHashCode();
                hash = (hash * 23) + this.ForwardsFromGrandma.GetHashCode();
                hash = (hash * 23) + this.SteelPlatedRollingPins.GetHashCode();
                hash = (hash * 23) + this.LubricatedDentures.GetHashCode();
                hash = (hash * 23) + this.Farms.GetHashCode();
                hash = (hash * 23) + this.CheapHoes.GetHashCode();
                hash = (hash * 23) + this.Fertilizer.GetHashCode();
                hash = (hash * 23) + this.CookieTrees.GetHashCode();
                hash = (hash * 23) + this.Mines.GetHashCode();
                hash = (hash * 23) + this.SugarGas.GetHashCode();
                hash = (hash * 23) + this.Megadrill.GetHashCode();
                hash = (hash * 23) + this.Factories.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return
                obj != null &&
                obj is State other &&
                this.Cursors == other.Cursors &&
                this.ReinforcedIndexFinger == other.ReinforcedIndexFinger &&
                this.CarpalTunnelPreventionCream == other.CarpalTunnelPreventionCream &&
                this.Ambidextrous == other.Ambidextrous &&
                this.ThousandFingers == other.ThousandFingers &&
                this.Grandmas == other.Grandmas &&
                this.ForwardsFromGrandma == other.ForwardsFromGrandma &&
                this.SteelPlatedRollingPins == other.SteelPlatedRollingPins &&
                this.LubricatedDentures == other.LubricatedDentures &&
                this.Farms == other.Farms &&
                this.CheapHoes == other.CheapHoes &&
                this.Fertilizer == other.Fertilizer &&
                this.CookieTrees == other.CookieTrees &&
                this.Mines == other.Mines &&
                this.SugarGas == other.SugarGas &&
                this.Megadrill == other.Megadrill &&
                this.Factories == other.Factories;
        }

        public static bool operator ==(State lhs, State rhs)
            => lhs.Equals(rhs);

        public static bool operator !=(State lhs, State rhs)
            => !(lhs == rhs);
    }
}