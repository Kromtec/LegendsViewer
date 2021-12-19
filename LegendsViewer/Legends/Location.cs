using System.Collections.Generic;
using System.Drawing;

namespace LegendsViewer.Legends
{
    public class Location : IEqualityComparer<Location>
    {
        // Quote
        //     Quote from: Max^TM
        //     myself I usually think of the 48x48 tile chunks as "embark tiles" due to their use on the embark screen/fort dimensions, 
        //     and the 16x16 embark tile chunks as "world tiles" accordingly. Do you have a more official--if you will--terminology that you use, 
        //     and are there different scales you use/think of the game world in terms of besides those?

        // Travel mode moves you basically on the embark tile scale and using dfhack in or near a site it's easy to see that altering 
        //     the coordinates of your traveling army by x+1 moves you over 1 "site travel" step, but it takes an x/y change of 3 or more 
        // to move your spot on the wilderness travel map, so you could call 1 embark tile 3 travel tiles or 16 dwarf scale tiles I guess?
        //     Quote from: PatrikLundell
        // - 16 * 16 world tiles for features
        // - 7 * 7 world tiles for "nearby" biome creatures
        // - 3 * 3 world tiles for region info
        // - 16 * 16 region tiles per world tile (called embark tiles by Max)
        //     - 3 * 3 tiles per region tile for various features, including "local features" (they don't cross these boundaries, currently). I've got no name for these.
        // - 48 * 48 embark tiles (in my terminology, dwarf scale in Max') per region tile.
        //     Quote from: Max^TM
        //     So for a 17x17 pocket world:
        // [w17]x[w17] in world tiles --world map export scale
        //     [r272]x[r272] in region tiles --wilderness travel scale
        //     [e816]x[e816] in embark tiles --near-site travel export
        //     [t39168]x[t39168] in local tiles --on foot scale

        //     Ha ha, I'm not sure official names are useful -- they are often the worst names since the purpose of the structures has changed over the years, etc.  
        //     In any case, we have the 16x16 world tiles, which are called "feature shells", since we used them first to handle groups of map features for save/load.  
        //     The 16x16 maps of a single world tile are called "midmaps", but there are 17x17 maps of sites at the same scale called "site realizations", and 
        //     those also have 51x51 versions which are blown up (and also exist in the site realization).  
        // Armies move at this scale (three times the midmap scale, but it doesn't have a name) 
        // -- generally the coordinates are stored as 
        // (ax,ay) for the world, 
        // (mmx,mmy) for the midmap, 
        // (smmx,smmy) for the three-times-closer army scale, and 
        //     (x,y,z) or (lx,ly,ly) for the local in-play tiles.  

        //     Aside from the 
        // "mm" for midmap and the 
        // "l" for local, the reasons are lost to time...  
        // "s" might be for site, and 
        // "a" might be for area.  

        //     A single midmap tile is the 48x48 play tile unit (doesn't have another name aside from "midmap tile"), and 
        //     it is made up of a 3x3 of 16x16x1 play tiles, and each of those 16x16x1s is a "block" (so a midmap tile is 3x3 blocks).  
        // The 16x16 is also sometimes taken with all Z levels as a "block column" (which handles cave-ins and some other data).  
        // Stuff like weather and biome creature areas are all one-off arrays/functions and not given useful names as far as I know.

        public int X { get; set; }
        public int Y { get; set; }
        public Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        public static bool operator ==(Location a, Location b)
        {
            if ((object)a == null && (object)b == null)
            {
                return true;
            }
            return (object)a != null && (object)b != null && a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Location a, Location b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return obj is Location b && X == b.X && Y == b.Y;
        }

        public override int GetHashCode()
        {
            return X ^ Y;
        }

        public bool Equals(Location b)
        {
            return X == b.X && Y == b.Y;
        }

        public bool Equals(Location a, Location b)
        {
            return a == b;
        }

        public int GetHashCode(Location location)
        {
            return location.X ^ location.Y;
        }
    }
}