using UnityEngine;

namespace Assets.Scripts
{
    static class RandomElement
    {

        public static void Shuffle(int[] deck)
        {
            for (int i = 0; i < deck.Length; i++)
            {
                int temp = deck[i];
                int randomIndex = Random.Range(0, deck.Length);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }

        public static void Shuffle(bool[] deck)
        {
            for (int i = 0; i < deck.Length; i++)
            {
                bool temp = deck[i];
                int randomIndex = Random.Range(0, deck.Length);
                deck[i] = deck[randomIndex];
                deck[randomIndex] = temp;
            }
        }


        //Choosing Items with Different Probabilities
        public static int Choose(float[] probs)
        {

            float total = 0;

            foreach (float elem in probs)
            {
                total += elem;
            }

            float randomPoint = Random.value * total;

            for (int i = 0; i < probs.Length; i++)
            {
                if (randomPoint < probs[i])
                {
                    return i;
                }
                else
                {
                    randomPoint -= probs[i];
                }
            }
            return probs.Length - 1;
        }

    }
}
