using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace DiscordBot {
    public class MathModule : CommandSet {
        public MathModule() {
            command = "math";
            shortHelp = "Math related commands. Works with floating point numbers.";
            catagory = Category.Advanced;
            requiredPermission = Permissions.Type.UseAdvancedCommands;

            commandsInSet = new Command [ ] {
                new Add (), new Subtract (), new Multiply (), new Divide (), new Pow (), new Log (), new Mod (), new Sin (), new Cos (), new Tan (), new ASin (), new ACos (), new ATan (), new Deg2Rad (), new Rad2Deg (), new PI (),
                new Round (), new Ceiling (), new Floor (), new Squareroot (), new Min (), new Max (), new Abs (), new Sign (), new Equal (), new Random (), new Graph (),
            };
        }

        public class Add : Command {
            public Add() {
                command = "add";

                AddOverload (typeof (double), "Add two numbers together.");
                AddOverload (typeof (double), "Get the sum of an array of numbers.");
                AddOverload (typeof (string), "Get the combination of two strings.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num1, double num2) {
                return TaskResult (num1 + num2, $"{num1} + {num2} = {num1 + num2}");
            }

            public Task<Result> Execute(SocketUserMessage e, double [ ] numbers) {
                return TaskResult (numbers.Sum (), $"Sum of given numbes: {numbers.Sum ()}");
            }

            public Task<Result> Execute(SocketUserMessage e, string str1, string str2) {
                return TaskResult (str1 + str2, str1 + str2);
            }
        }

        public class Subtract : Command {

            public Subtract() {
                command = "subtract";

                AddOverload (typeof (double), "Subtract num2 from num1.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num1, double num2) {
                return TaskResult (num1 - num2, $"{num1} - {num2} = {num1 - num2}");
            }
        }

        public class Multiply : Command {

            public Multiply() {
                command = "multiply";

                AddOverload (typeof (double), "Mutliply two numbers.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num1, double num2) {
                return TaskResult (num1 * num2, $"{num1} * {num2} = {num1 * num2}");
            }
        }

        public class Divide : Command {

            public Divide() {
                command = "divide";

                AddOverload (typeof (double), "Divide num1 with num2.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num1, double num2) {
                return TaskResult (num1 / num2, $"{num1} / {num2} = {num1 / num2}");
            }
        }

        public class Pow : Command {

            public Pow() {
                command = "pow";

                AddOverload (typeof (double), "Get the num1 to the power of num2.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num1, double num2) {
                return TaskResult (Math.Pow (num1, num2), $"{num1}^{num2} = {Math.Pow (num1, num2)}");
            }
        }

        public class Log : Command {

            public Log() {
                command = "log";

                AddOverload (typeof (double), "Get the natural logarithm of the given number.");
                AddOverload (typeof (double), "Get the logarithm of the given number in a specific base.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Log (num), $"Log({num}) = {Math.Log (num)}");
            }

            public Task<Result> Execute(SocketUserMessage e, double num, double logBase) {
                return TaskResult (Math.Log (num, logBase), $"Log{logBase}({num}) = {Math.Log (num, logBase)}");
            }
        }

        public class Mod : Command {

            public Mod() {
                command = "mod";

                AddOverload (typeof (double), "Get the remainder of num1 / num2.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num1, double num2) {
                return TaskResult (num1 % num2, $"{num1} % {num2} = {num1 % num2}");
            }
        }

        public class Sin : Command {

            public Sin() {
                command = "sin";

                AddOverload (typeof (double), "Get the sin of the given angle in radians.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Sin (num), $"SIN ({num}) = {Math.Sin (num)}");
            }
        }

        public class Cos : Command {

            public Cos() {
                command = "cos";

                AddOverload (typeof (double), "Get the cos of the given angle in radians.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Cos (num), $"COS ({num}) = {Math.Cos (num)}");
            }
        }

        public class Tan : Command {

            public Tan() {
                command = "tan";

                AddOverload (typeof (double), "Get the tan of the given angle in radians.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Tan (num), $"TAN ({num}) = {Math.Tan (num)}");
            }
        }

        public class ASin : Command {

            public ASin() {
                command = "asin";

                AddOverload (typeof (double), "Get the inverse sin of the given value in radians.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Asin (num), $"ASIN ({num}) = {Math.Asin (num)}");
            }
        }

        public class ACos : Command {

            public ACos() {
                command = "acos";

                AddOverload (typeof (double), "Get the inverse cos of the given value in radians.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Acos (num), $"ACOS ({num}) = {Math.Acos (num)}");
            }
        }

        public class ATan : Command {

            public ATan() {
                command = "atan";

                AddOverload (typeof (double), "Get the atan of the given value in radians.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Atan (num), $"TAN ({num}) = {Math.Atan (num)}");
            }
        }

        public class Deg2Rad : Command {
            public Deg2Rad() {
                command = "degtorad";

                AddOverload (typeof (double), "Convert the given degrees to radians.");
            }

            public Task<Result> Execute(SocketUserMessage e, double degs) {
                return TaskResult (degs / (180d / Math.PI), $"DEG2RAD ({degs}) = {degs / (180d / Math.PI)}");
            }
        }

        public class Rad2Deg : Command {
            public Rad2Deg() {
                command = "radtodeg";

                AddOverload (typeof (double), "Convert the given radians to degrees.");
            }

            public Task<Result> Execute(SocketUserMessage e, double rads) {
                return TaskResult (rads * (180d / Math.PI), $"RAD2DEG ({rads}) = {rads * (180d / Math.PI)}");
            }
        }

        public class PI : Command {
            public PI() {
                command = "pi";

                AddOverload (typeof (double), "Returns pi.");
            }

            public Task<Result> Execute(SocketUserMessage e) {
                return TaskResult (Math.PI, "PI = " + Math.PI);
            }
        }

        public class Round : Command {
            public Round() {
                command = "round";

                AddOverload (typeof (double), "Rounds given input to the nearest whole number.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Round (num), $"ROUND ({num}) = {Math.Round (num)}");
            }
        }

        public class Floor : Command {
            public Floor() {
                command = "floor";

                AddOverload (typeof (double), "Floors given input to the nearest whole number below itself.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Floor (num), $"FLOOR ({num}) = {Math.Floor (num)}");
            }
        }

        public class Ceiling : Command {
            public Ceiling() {
                command = "ceiling"; 

                AddOverload (typeof (double), "Ceils given input to the nearest whole number above itself.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Ceiling (num), $"ROUND ({num}) = {Math.Ceiling (num)}");
            }
        }

        public class Squareroot : Command {
            public Squareroot() {
                command = "sqrt";

                AddOverload (typeof (double), "Returns the square root of the given number.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Sqrt (num), $"SQRT ({num}) = {Math.Sqrt (num)}");
            }
        }

        public class Min : Command {
            public Min() {
                command = "min";

                AddOverload (typeof (double), "Returns the lowest number of the given array.");
            }

            public Task<Result> Execute(SocketUserMessage e, params double [ ] nums) {
                return TaskResult (nums.Min (), $"Min of given numbers: {nums.Min ()}");
            }
        }

        public class Max : Command {
            public Max() {
                command = "max";

                AddOverload (typeof (double), "Returns the highest number of the given array.");
            }

            public Task<Result> Execute(SocketUserMessage e, params double [ ] nums) {
                return TaskResult (nums.Max (), $"Max of given numbers: {nums.Max ()}");
            }
        }

        public class Abs : Command {
            public Abs() {
                command = "abs";

                AddOverload (typeof (double), "Returns the absolute number of the given array.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Abs (num), $"ABS ({num}) = {Math.Abs (num)}");
            }
        }

        public class Sign : Command {
            public Sign() {
                command = "sign";

                AddOverload (typeof (double), "Returns the sign of the given number.");
            }

            public Task<Result> Execute(SocketUserMessage e, double num) {
                return TaskResult (Math.Sign (num), $"SIGN ({num}) = {Math.Sign (num)}");
            }
        }

        public class Equal : Command {
            public Equal() {
                command = "equals";

                AddOverload (typeof (bool), "Returns true if given objects are the same.");
            }

            public Task<Result> Execute(SocketUserMessage e, object obj1, object obj2) {
                return TaskResult (obj1.Equals(obj2), $"{obj1} EQUALS {obj2} = {obj1.Equals (obj2)}");
            }
        }

        public class Random : Command {
            public Random() {
                command = "random";

                AddOverload (typeof (double), "Returns random number between 0 and 1.");
                AddOverload (typeof (bool), "Returns random number between 0 and given number.");
                AddOverload (typeof (bool), "Returns random number between the given numbers.");
            }

            public Task<Result> Execute(SocketUserMessage e) {
                System.Random random = new System.Random ();
                return TaskResult (random.NextDouble (), "");
            }

            public Task<Result> Execute(SocketUserMessage e, double max) {
                System.Random random = new System.Random ();
                return TaskResult (random.NextDouble () * max, "");
            }

            public Task<Result> Execute(SocketUserMessage e, double min, double max) {
                System.Random random = new System.Random ();
                return TaskResult (random.NextDouble () * (max + min) - min, "");
            }
        }

        public class Graph : Command {

            public const int X_RES = 512, Y_RES = 512;

            public Graph() {
                command = "graph";

                AddOverload (typeof (object), "Draw a graph of the given function within the given range.");
            }

            public async Task<Result> Execute(SocketUserMessage e, double xrange, double yrange, string yequals) {
                double xstart = -xrange / 2d;
                double xend = xrange / 2d;

                double ystart = yrange / 2d;
                double yend = -yrange / 2d;

                // These are actually the inverse scale, and should be the other way around. Though at this point it doesn't really matter.
                double xscale = (xend - xstart) / X_RES;
                double yscale = (yend - ystart) / Y_RES;

                string cmd;
                List<string> args;
                if (TryIsolateWrappedCommand (yequals, out cmd, out args)) {

                    try {
                        using (Bitmap bitmap = new Bitmap (X_RES, Y_RES)) {
                            int yprev = 0;
                            for (int y = 0; y < Y_RES; y++) {
                                for (int x = 0; x < X_RES; x++) {

                                    if ( 
                                        x % (int)Math.Abs (X_RES / xrange) == 0 || 
                                        y % (int)Math.Abs (Y_RES / yrange) == 0
                                        )
                                        bitmap.SetPixel (x, y, Color.LightGray);
                                    else if (y == Y_RES / 2 || x == X_RES / 2) {
                                        bitmap.SetPixel (x, y, Color.Gray);
                                    } else {
                                        bitmap.SetPixel (x, y, Color.White);
                                    }
                                }
                            }

                            try {
                                using (Graphics graphics = Graphics.FromImage (bitmap)) {
                                    using (Font font = new Font ("Areal", 16)) {
                                        graphics.DrawString ($"({xstart},{ystart})", font, Brushes.Gray, 0, 0);

                                        SizeF lowerSize = graphics.MeasureString ($"({xend},{yend})", font);
                                        graphics.DrawString ($"({xend},{yend})", font, Brushes.Gray, 512 - lowerSize.Width, 512 - lowerSize.Height);
                                    }
                                }
                            }catch { };

                            for (double x = xstart; x < xend; x += xscale) {

                                int xpix = (int)Math.Round (x / xscale) + X_RES / 2;
                                System.Tuple<int, bool> res = await CalcY (e, cmd, args, x, xscale, yscale);
                                int ycur = res.Item1;

                                if (res.Item2) { // Is the result defined?
                                    int dist = ycur - yprev;
                                    int sign = Math.Sign (dist);
                                    dist = Math.Min (Math.Abs (dist), X_RES);
                                    if (dist == 0)
                                        dist = 1;

                                    for (int yy = 0; yy < dist; yy++) {
                                        double fraction = yy / (double)dist * xscale;
                                        System.Tuple<int, bool> locRes = await CalcY (e, cmd, args, x + fraction, xscale, yscale);
                                        if (!locRes.Item2)
                                            break;

                                        int ypix = locRes.Item1;

                                        if (!(
                                            xpix < 0 || xpix >= X_RES ||
                                            ypix < 0 || ypix >= Y_RES
                                            ))
                                            bitmap.SetPixel (xpix, ypix, Color.Black);
                                    }

                                    yprev = ycur;
                                }
                            }

                            using (MemoryStream stream = new MemoryStream ()) {
                                bitmap.Save (stream, System.Drawing.Imaging.ImageFormat.Png);
                                stream.Position = 0;
                                await Program.messageControl.SendImage (e.Channel as SocketTextChannel, "", stream, "graph.png", allowInMain);
                            }

                            return new Result (null, "");
                        }
                    } catch (Exception exception) {
                        Logging.Log (exception);
                    }
                }

                return new Result (null, "Function failed, function command might not be a mathematical one.");
            }

            private async Task<System.Tuple<int, bool>> CalcY(SocketUserMessage e, string cmd, List<string> args, double x, double xscale, double yscale) {
                CommandVariables.Set (e.Id, "x", x, true);

                Program.FoundCommandResult result = await Program.FindAndExecuteCommand (e, cmd, args, Program.commands, 1, false, true);
                double y = (double)Convert.ChangeType (result.result.value, typeof (double));

                int ycur = (int)Math.Round (y / yscale) + Y_RES / 2;

                return new System.Tuple<int, bool> (ycur, !double.IsNaN (y));
            }
        }
    }
}
