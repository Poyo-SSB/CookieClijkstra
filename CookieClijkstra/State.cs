using System;

namespace CookieClijkstra
{
    public struct State
    {
        private const float cost_foo = 10;
        private const float cost_bar = 80;

        public const int COST_FOO_UPGRADE = 100;
        public const int COST_BAR_UPGRADE = 400;

        public byte Foos;
        public int FooCost => Cost(cost_foo, this.Foos);

        public bool FooUpgrade;
        public bool FooUpgradeAvailable => !(this.FooUpgrade || this.Foos < 1);

        public byte Bars;
        public int BarCost => Cost(cost_bar, this.Bars);

        public bool BarUpgrade;
        public bool BarUpgradeAvailable => !(this.BarUpgrade || this.Bars < 1);

        public float ProductionRate
        {
            get
            {
                float baseProduction = 1;

                float fooProduction = 2 * this.Foos;
                float barProduction = 20 * this.Bars;

                if (this.FooUpgrade)
                {
                    fooProduction *= 2;
                }
                if (this.BarUpgrade)
                {
                    barProduction *= 2;
                }

                return baseProduction + fooProduction + barProduction;
            }
        }

        public int AlreadySpent
        {
            get
            {
                int fooCost = Sum(cost_foo, this.Foos);
                int barCost = Sum(cost_bar, this.Bars);

                int totalDucks = fooCost + barCost;

                totalDucks += this.FooUpgrade ? COST_FOO_UPGRADE : 0;
                totalDucks += this.BarUpgrade ? COST_BAR_UPGRADE : 0;

                return totalDucks;
            }
        }

        private static int Cost(float baseCost, int ownedCount)
            =>(int)Math.Ceiling(baseCost * Math.Pow(1.15, ownedCount));

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
            this.Foos = other.Foos;
            this.FooUpgrade = other.FooUpgrade;
            this.Bars = other.Bars;
            this.BarUpgrade = other.BarUpgrade;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = (hash * 23) + this.Foos.GetHashCode();
                hash = (hash * 23) + this.FooUpgrade.GetHashCode();
                hash = (hash * 23) + this.Bars.GetHashCode();
                hash = (hash * 23) + this.BarUpgrade.GetHashCode();
                return hash;
            }
        }

        public override bool Equals(object obj)
        {
            return
                obj != null &&
                obj is State other &&
                this.Foos == other.Foos &&
                this.FooUpgrade == other.FooUpgrade &&
                this.Bars == other.Bars &&
                this.BarUpgrade == other.BarUpgrade;
        }

        public static bool operator ==(State lhs, State rhs)
            => lhs.Equals(rhs);

        public static bool operator !=(State lhs, State rhs)
            => !lhs.Equals(rhs);
    }
}