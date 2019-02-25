using System;
using System.Linq;

namespace Astrophysical_Console
{
    public class Coordinates
    {
        private int _raH;
        private int _raM;
        private int _raS;
        private int _decD;
        private int _decM;
        private int _decS;
        
        public Coordinates(int raH, int raM, int raS, int decD, int decM, int decS)
        {
            this._raH = raH;
            this._raM = raM;
            this._raS = raS;
            this._decD = decD;
            this._decM = decM;
            this._decS = decS;
        }

        public Coordinates(int raSeconds, int decSeconds)
        {
            _raH = raSeconds / 3600;
            _raM = (raSeconds % 3600) / 60;
            _raS = raSeconds - 3600 * _raH - 60 * _raM;

            _decD = decSeconds / 3600;
            _decM = (decSeconds % 3600) / 60;
            _decS = decSeconds - _decD * 3600 - _decM * 60;
        }

        public Coordinates(string ra, string dec, char splitChar = '+')
        {
            int[] raArr = ra.Split(splitChar).Select(x => int.Parse(x)).ToArray();
            int[] decArr = dec.Split(splitChar).Select(x => int.Parse(x)).ToArray();

            _raH = raArr[0];
            _raM = raArr[1];
            _raS = raArr[2];
            _decD = decArr[0];
            _decM = decArr[1];
            _decS = decArr[2];
        }

        public Coordinates(string coords, char splitChar = '+')
        {
            int[] coords1 = coords.Split(splitChar).Select(x => int.Parse(x)).ToArray();

            _raH = coords1[0];
            _raM = coords1[1];
            _raS = coords1[2];
            _decD = coords1[3];
            _decM = coords1[4];
            _decS = coords1[5];
        }

        /// <summary>
        /// Coordinate's right ascension hours value
        /// </summary>
        public int RAH
        {
            get => _raH;
            set
            {
                if (value >= 0 && value < 24)
                    _raH = value;
                else throw new InvalidOperationException("Right ascension hours value can not be more than 24 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's right ascension minutes value
        /// </summary>
        public int RAM
        {
            get => _raM;
            set
            {
                if (value >= 0 && value < 60)
                    _raM = value;
                else throw new InvalidOperationException("Right ascension minutes value can not be more than 60 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's right ascension seconds value
        /// </summary>
        public int RAS
        {
            get => _raS;
            set
            {
                if (value >= 0 && value < 60)
                    _raS = value;
                else throw new InvalidOperationException("Right ascension seconds value can not be more than 60 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's decline degrees value
        /// </summary>
        public int DecD
        {
            get => _decD;
            set
            {
                if (value >= 0 && value < 360)
                    _decD = value;
                else throw new InvalidOperationException("Decline degrees value can not be more than 360 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's decline minutes value
        /// </summary>
        public int DecM
        {
            get => _decM;
            set
            {
                if (value >= 0 && value < 60)
                    _decM = value;
                else throw new InvalidOperationException("Decline minutes value can not be more than 60 and less than 0.");
            }
        }
        /// <summary>
        /// Coordinate's decline seconds value
        /// </summary>
        public int DecS
        {
            get => _decS;
            set
            {
                if (value >= 0 && value < 60)
                    _decS = value;
                else throw new InvalidOperationException("Decline seconds value can not be more than 60 and less than 0.");
            }
        }

        /// <summary>
        /// Converts RA coordinate to RA seconds
        /// </summary>
        public int RASeconds => RAH * 3600 + RAM * 60 + RAS;
        /// <summary>
        /// Converts decline coordinate to decline seconds
        /// </summary>
        public int DecSeconds => DecD * 3600 + DecM * 60 + DecS;
        
        public override bool Equals(object obj1)
        {
            Coordinates obj = (Coordinates)obj1;

            if (RAH == obj.RAH &&
                RAM == obj.RAM &&
                RAS == obj.RAS &&
                DecD == obj.DecD &&
                DecM == obj.DecM &&
                DecS == obj.DecS)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return RAH.GetHashCode() ^ RAM.GetHashCode() ^ RAS.GetHashCode() ^ DecD.GetHashCode() ^ DecM.GetHashCode() ^ DecS.GetHashCode();
        }

        public override string ToString()
        {
            return RAH + "+" + RAM + "+" + RAS + "+" + DecD + "+" + DecM + "+" + DecS;
        }

        public string RAToString()
        {
            return RAH + "+" + RAM + "+" + RAS;
        }

        public string DecToString()
        {
            return DecD + "+" + DecM + "+" + DecS;
        }

        public static Coordinates operator +(Coordinates c1, Coordinates c2)
        {
            int raSecs = (c1.RAS + c2.RAS) % 60;
            int raMins = ((c1.RAS + c2.RAS) / 60 + c1.RAM + c2.RAM) % 60;
            int raHours = (((c1.RAS + c2.RAS) / 60 + c1.RAM + c2.RAM) / 60 + c1.RAH + c2.RAH) % 24;

            int decSecs = (c1.DecS + c2.DecS) % 60;
            int decMins = ((c1.DecS + c2.DecS) / 60 + c1.DecM + c2.DecM) % 60;
            int decDegrees = (((c1.DecS + c2.DecS) / 60 + c1.DecM + c2.DecM) / 60 + c1.DecD + c2.DecD) % 180;

            return new Coordinates(raHours, raMins, raSecs, decDegrees, decMins, decSecs);
        }
        public static Coordinates operator -(Coordinates c1, Coordinates c2)
        {
            int buffer = c1.RAS - c2.RAS;
            int raSecs = Residual(buffer, 60);
            buffer = (buffer < 0) ? c1.RAM - c2.RAM - 1 : c1.RAM - c2.RAM;
            int raMins = Residual(buffer, 60);
            buffer = (buffer < 0) ? c1.RAH - c2.RAH - 1 : c1.RAH - c2.RAH;
            int raHours = Residual(buffer, 24);

            buffer = c1.DecS - c2.DecS;
            int decSecs = Residual(buffer, 60);
            buffer = (buffer < 0) ? c1.DecM - c2.DecM - 1 : c1.DecM - c2.DecM;
            int decMins = Residual(buffer, 60);
            buffer = (buffer < 0) ? c1.DecD - c2.DecD - 1 : c1.DecD - c2.DecD;
            int decDegrees = Residual(buffer, 180);

            return new Coordinates(raHours, raMins, raSecs, decDegrees, decMins, decSecs);
        }

        public static double Distance(Coordinates c1, Coordinates c2) => Math.Sqrt(Math.Pow((c2.RASeconds - c1.RASeconds) * 15, 2) + Math.Pow((c2.DecSeconds - c1.DecSeconds), 2));

        public static Coordinates Middle(Coordinates c1, Coordinates c2)
        {
            int deltaRA = (c2.RASeconds - c1.RASeconds) / 2,
                deltaDec = (c2.DecSeconds - c1.DecSeconds) / 2;

            return c1 + new Coordinates(deltaRA, deltaDec);
        }

        private static int Residual(int n, int d)
        {
            int result = n % d;
            if (Math.Sign(result) * Math.Sign(d) < 0)
                result += d;
            return result;
        }
    }
}
