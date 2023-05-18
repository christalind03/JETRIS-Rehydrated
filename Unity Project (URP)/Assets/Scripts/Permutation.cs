using System;

class Permutation
{
    public void GetRandom(int[] data, int randomSeed)
    {
        Random random = new Random(randomSeed);
        int dataLength = data.Length;

        while(dataLength > 1)
        {
            int randomNum = random.Next(dataLength--);
            Swap(data, dataLength, randomNum);
        }
    }

    private void Swap(int[] data, int indexOne, int indexTwo)
    {
        int tempValue = data[indexOne];
        data[indexOne] = data[indexTwo];
        data[indexTwo] = tempValue;
    }
}