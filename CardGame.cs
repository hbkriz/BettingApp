using System;
using System.Threading;

public class CardGame 
{
public static void SimulateAndShufffle() {
     
        int[] Deck = new int[52];

     Random RNG = new Random();
//52 card
        for (int x = 0; x < 52; x++)
        {
            Deck[x] = x;
        }

//Shuffle cards
        for (int x = 0; x < 52; x++)
        {
            int r = RNG.Next(52);
            
            if (r != x)
            {
                int tmp = Deck[x];
                Deck[x] = Deck[r];
                Deck[r] = tmp;
            }
        }
}

   
}