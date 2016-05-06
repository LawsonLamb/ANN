using System;
using System.Collections.Generic;
namespace Project3
{
	public class Layer

	{
		public Node[] Nodes;
		public double [] Output;
		public double[] Input;
		

		public Layer (int NumberOfNodes,int numberOfInputs)
		{
			Nodes = new Node[NumberOfNodes];
			Output = new double[NumberOfNodes];
			Input = new double[numberOfInputs];

			for (int i = 0; i < Nodes.Length; i++) {
				Nodes [i] = new Node (numberOfInputs);


			}

		
		}

		public Layer(){
			

		}


		public void FowardFeed(){
			double Sum = 0;
			for (int x = 0; x < Nodes.Length; x++) {


			 Sum = Nodes [x].Threshold;

				for (int y = 0; y < Nodes.Length; y++) {

					Sum = Sum +Input[y] * Nodes[x].weight[y];


				}
						

			Nodes [x].Output = Sigmoid (Sum);

			
				Output [x] = Nodes [x].Output;
			

			
				
	}

	
}
		public double Sigmoid(double sum){
			return 1 / (1 + Math.Exp (-sum));

		}


        public void PrintNodeWeights()
        {


            for(int i=0; i < Nodes.Length; i++)
            {
                Console.WriteLine("Node: " + i);
                Nodes[i].PrintWeights();

            }
        }

		public int findLargestNode(){
			int largestIndex = 0;
			double largestValue = 0;
			for (int i = 0; i < Nodes.Length; i++) {

				if (largestValue < Nodes [i].Output) {

					largestValue = Nodes [i].Output;
					largestIndex = i;
				} 
			}
			return largestIndex;


		}
}
}

