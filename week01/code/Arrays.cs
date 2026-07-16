public static class Arrays
{
    /// <summary>
    /// This function will produce an array of size 'length' starting with 'number' followed by multiples of 'number'.  For 
    /// example, MultiplesOf(7, 5) will result in: {7, 14, 21, 28, 35}.  Assume that length is a positive
    /// integer greater than 0.
    /// </summary>
    /// <returns>array of doubles that are the multiples of the supplied number</returns>
    public static double[] MultiplesOf(double number, int length)
    {
        // Plan:
        // 1. Create a new array of doubles with size 'length'.
        // 2. Use a for-loop from i = 0 to i < length.
        // 3. For each index i, compute the (i+1)th multiple of 'number' as number * (i+1)
        //    and store it in results[i]. (We use i+1 because the first multiple is the number itself.)
        // 4. After the loop completes, return the results array.

        double[] results = new double[length];
        for (int i = 0; i < length; i++) {
            results[i] = number * (i + 1);
        }

        return results;
    }

    /// <summary>
    /// Rotate the 'data' to the right by the 'amount'.  For example, if the data is 
    /// List<int>{1, 2, 3, 4, 5, 6, 7, 8, 9} and an amount is 3 then the list after the function runs should be 
    /// List<int>{7, 8, 9, 1, 2, 3, 4, 5, 6}.  The value of amount will be in the range of 1 to data.Count, inclusive.
    ///
    /// Because a list is dynamic, this function will modify the existing data list rather than returning a new list.
    /// </summary>
    public static void RotateListRight(List<int> data, int amount)
    {
        // Plan:
        // 1. Determine the split index where the list will be divided. This is data.Count - amount.
        // 2. Extract the tail portion that should move to the front using GetRange(split, amount).
        // 3. Extract the head portion using GetRange(0, split).
        // 4. Clear the original list and AddRange the tail followed by the head to produce the rotated list.
        // 5. Edge cases: if data is null or empty, do nothing. If amount equals data.Count,
        //    the resulting order is the same as the original, and the algorithm still works.

        if (data == null) return;
        int n = data.Count;
        if (n == 0) return;

        // amount is guaranteed to be in range 1..data.Count inclusive, but handle general cases safely
        amount = amount % n;
        if (amount == 0) return; // no rotation needed

        int split = n - amount;
        var tail = data.GetRange(split, amount);
        var head = data.GetRange(0, split);

        data.Clear();
        data.AddRange(tail);
        data.AddRange(head);
    }
}
