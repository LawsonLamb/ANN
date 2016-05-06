using System;
using System.Collections.Generic;

namespace Project3
{
	public class Node
	{	public double[] weight;
		public double Output;
		public double[] weightDiff;
		public double SignalError;
		public double Threshold;
		public double ThresholdDiff;
		public double input;

		public Node (int InputNumber)
		{
			weight = new double[InputNumber];
			ThresholdDiff = 0;
			weightDiff = new double[InputNumber];
			Random ran = new Random ();
         
			//Threshold = 1;
			Threshold = ran.NextDouble () * 0.09 + 0.01;
			for (int i = 0; i < weight.Length; i++) {
				
					weightDiff [i] = 0;
				//weight[i] = (double)ran.Next(-1,2);
				weight [i] = ran.NextDouble () * 0.09 + 0.01;
			}

		}
		public Node(){

		}

		public void Process(double[] input){

			double sum = Threshold;

			for (int i = 0; i < input.Length; i++) {

				sum += input [i] * weight [i];


			}
		

			Output = Sigmoid (sum);
		

		}
		public double Sigmoid(double sum){
			return 1 / (1 + Math.Exp (-sum));

		}

        
        public void PrintWeights()
        {
            Console.WriteLine("Weights");

            for (int i = 0;i< weight.Length; i++)
            {
                Console.WriteLine(i +" :" + weight[i]);

            }
        

        }




    }
}

