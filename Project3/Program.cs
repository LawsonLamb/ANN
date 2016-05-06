using System;

namespace Project3
{
	class MainClass
	{	static Network Network;
		public static void Main (string[] args)
		{ int [] Layers = {64,32,16,10};
			Network = new Network (Layers);
            //Network.ParseTraining ("/Users/ItBNinja/Projects/Project3/Project3/optdigits_test.txt");
			Network.ParseTraining ("/Users/ItBNinja/Desktop/Project3/Project3/optdigits_train.txt");
           	Network.TrainNetwork();

            Console.WriteLine("ENTER TO CLOSE");
            Console.ReadKey();

        }
    }
}
