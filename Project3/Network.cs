using System;
using System.Collections.Generic;
using System.IO;
namespace Project3
{
    public class Network
    {
        public Layer[] Layers;
        private int SampleNumber;
        private int NumberOfSamples;
        private double OverallError;
        private double MinimumError;
        private double[,] ExpectedOutput;
        private double[,] ActualOutput;


        private double LearningRate;
        private double Momentum;

		public int[,] Output;
        //	private int NumberOfLayers;
        private double[,] Input;
        int[] NumberOfLayers;
		int OutputLayer;
        public Network(int[] numberOfLayers)
		{
			NumberOfLayers = numberOfLayers;
			OutputLayer = NumberOfLayers.Length - 1;
			MinimumError = .5;
			LearningRate = .01;
			Momentum = .01;


			Layers = new Layer[NumberOfLayers.Length];

			Layers [0] = new Layer (NumberOfLayers [0], numberOfLayers [0]);

			for (int n = 1; n < Layers.Length; n++) {

				Layers [n] = new Layer (numberOfLayers [n], numberOfLayers [n - 1]);


				//Console.WriteLine ("LAYER : " + n + " Nodes : "  + numberOfLayers[n]+ " NumberofInputs: "+ numberOfLayers[n-1]);

			

			}
			setOutputValues ();

		}



		public void setOutputValues(){


			Output = new int[10, 10];


			for (int x = 0; x < Output.GetLength (0); x++) {
				for (int y = 0; y < Output.GetLength (1); y++) {
					if (x == y) {
						Output [x, y] = 1;
					} else {
						Output [x, y] = 0;
					}
				

				}

			}

			 
		}



        public double ErrorSignal(double expectedOutput, double actualOutput) {

            return (expectedOutput - actualOutput) * ((actualOutput) * (1 - actualOutput));
        }



        private void CalculateOverallError() {
            int i;

            OverallError = 0;

            for (i = 0; i < NumberOfSamples; i++)
			{	for (int j=0; j < Layers [OutputLayer].Nodes.Length; j++)

				OverallError = OverallError + (0.5 * (Math.Pow(ExpectedOutput[i, j] - ActualOutput[i, j], 2)));

            }

        }

	


        private void BackPropagateError() {

            for (int i = NumberOfLayers.Length - 1; i > 0; i--) {
                for (int j = 0; j < Layers[i].Nodes.Length; j++) {


					  //calculate biasweight differnce to node
					Layers[i].Nodes[j].ThresholdDiff = WeightDiff(Layers[i].Nodes[j].SignalError,Layers[i].Nodes[j].ThresholdDiff);
					Layers [i].Nodes [j].Threshold = UpdateWeight (Layers [i].Nodes [j].Threshold, Layers [i].Nodes [j].ThresholdDiff);
                    for (int k = 0; k < Layers[i].Input.Length; k++) {


                        // Update Weights
						Layers [i].Nodes [j].weightDiff [k] =  WeightDiff(Layers[i].Nodes[j].SignalError,Layers[i-1].Nodes[j].Output,Layers [i].Nodes [j].weightDiff [k]);
						Layers [i].Nodes [j].weight [k] = UpdateWeight (Layers [i].Nodes [j].weight [k], Layers [i].Nodes [j].weightDiff [k]);

                    }
                }

            }

        }


		private double WeightDiff(double SE,double WD){

			return LearningRate * SE + Momentum* WD;
		}
		private double WeightDiff(double SE,double OP,double WD){

			return LearningRate * SE * OP * Momentum * WD;
		}
		private double UpdateWeight(double W,double WD){


			return W + WD;
		}


        private void CalculateSignalErrors() {
          
			double Sum;
			int i,j;

			for (i = 0; i < Layers [OutputLayer].Nodes.Length; i++) {
				Layers [OutputLayer].Nodes [i].SignalError = ErrorSignal (ExpectedOutput[SampleNumber,i], Layers [OutputLayer].Nodes [i].Output);
			}

       
            for ( i = NumberOfLayers.Length - 2; i > 0; i--) {
                for ( j = 0; j < Layers[i].Nodes.Length; j++) {
                    Sum = 0;
                    for (int k = 0; k < Layers[i + 1].Nodes.Length; k++) {
                        Sum = Sum + Layers[i + 1].Nodes[k].weight[j] * Layers[i + 1].Nodes[k].SignalError;
                    }
					Layers[i].Nodes[j].SignalError = Layers[i].Nodes[j].Output * (1 - Layers[i].Nodes[j].Output) * Sum;

                }

            }
        }

        private void FowardFeed() {

            //Console.WriteLine ("LAYER : " + 0);

            Layers[0].FowardFeed();

            for (int n = 1; n < Layers.Length; n++) {
                //	Console.WriteLine ("LAYER : " + n );
				for (int i = 0; i <Layers [n - 1].Output.Length; i++) {
					Layers [n].Input[i] = Layers [n-1].Output[i];
				}
                Layers[n].FowardFeed();


            }




        }


        public void ParseTraining(String filename) {


            string[] fileData = File.ReadAllLines(@filename);
            NumberOfSamples = fileData.Length;
           // NumberOfSamples = 9;
            Input = new double[NumberOfSamples, 64];
            ExpectedOutput = new double[NumberOfSamples, 10];
            ActualOutput = new double[NumberOfSamples, 10];

			int x;
            for (int j = 0; j < NumberOfSamples; j++) {

                string[] s = fileData[j].Split(',');


                for ( x = 0; x < 64; x++) {


                    int value = Int32.Parse(s[x]);
                    Input[j, x] = (double)value;
                    


                    //	Console.WriteLine ("x: " + x + " , "  + "value: " + value);




                }

				for(x=0; x<10;x++){

				
					ExpectedOutput[j, x]= 	Output[Int32.Parse(s[64]),x];
				
				}

            }

        




        }


        public void TrainNetwork() {
            int i;
            int count = 0;
            do
            {
				

                for (SampleNumber = 0; SampleNumber < NumberOfSamples; SampleNumber++)
                {
                    for (i = 0; i < Layers[0].Nodes.Length; i++)
                        Layers[0].Input[i] = Input[SampleNumber, i];

                    FowardFeed();

					for( i=0;i<10;i++){

                    ActualOutput[SampleNumber, i] = Layers[NumberOfLayers.Length - 1].Output[i];

					}
					//PrintOutput();
					//PrintLayerWeights();
					//Console.ReadKey();
                    CalculateSignalErrors();
                    BackPropagateError();
					//CheckWeights();
                }
			//PrintOutput();
			//	PrintLayerWeights();
			//	Console.ReadKey();
				CalculateOverallError();
     //  	 PrintOutput();
              // PrintLayerWeights();

                count++;
                Console.WriteLine(count + " Overall Error : " + OverallError + " ,  Min Error: " + MinimumError);
				//CheckWeights();
             // Console.ReadKey();

		
            }

            while (OverallError > MinimumError);



        }

	void CheckWeights(){
			int index = 0;
			int match = 0;

			for (int i = 0; i < 10; i++) {
				index++;
				
				if (ExpectedOutput [SampleNumber, i] == 1) {

					int largest =	Layers [NumberOfLayers.Length - 1].findLargestNode ();
					if (largest == index) {
						//Console.WriteLine ("EXPECTED OUTPUT: " + index + " ACTUAL OUTPUT: " + largest);
						match++;
					}
					//	Console.WriteLine ("EXPECTED OUTPUT: " + index + " ACTUAL OUTPUT: " + largest);
					index = 0;

				}

				


			}
			float percentage = match / NumberOfSamples;
			//percentage;
			Console.WriteLine (" MATCHED NUMBER: " + match); 

			

		}



        public  void PrintOutput()
        {
			
			for (int x = 2; x < 3; x++) {
				Console.WriteLine ("Sample Number: " + x);
				for (int y = 0; y < 10; y++) {
					Console.WriteLine ( " Output : " + ActualOutput [x, y] + ", Expected Output: " + ExpectedOutput [x, y]);
				}

			}

			/*
		for (int y = 0; y < 10; y++) {
			Console.WriteLine ( " Output : " + ActualOutput [SampleNumber, y] + ", Expected Output: " + ExpectedOutput [SampleNumber, y]);
		}
		*/

			/*for (int x = 0; x < NumberOfLayers.Length; x++){
				Console.WriteLine ("Layer:" + x);

				for (int y = 0; y < Layers [x].Nodes.Length; y++) {

					Console.WriteLine (y + ": Output: " + Layers [x].Nodes [y].Output);

				}

			}
*/
        }
        public void PrintLayerWeights()
        {

            for(int i=0; i < NumberOfLayers.Length; i++)
            {
                Console.WriteLine("Layer: " + i);


				Layers[NumberOfLayers.Length-1].PrintNodeWeights();
           }
        }









		}






}





