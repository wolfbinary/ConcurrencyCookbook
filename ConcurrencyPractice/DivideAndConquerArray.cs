public class DivideAndConquerArray
{


    public List<string> MakeFakeDirsList(int numberOfFakeStrings = 9551, int randomDirsLength = 10){
        var fakeDirsList = new List<string>();
        var random = new Random();

        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        
        for(int i = 0; i < numberOfFakeStrings; i++)
        {
            var fakeDir = new string(Enumerable.Repeat(chars, randomDirsLength)
                .Select(s => s[random.Next(s.Length)]).ToArray());
            fakeDirsList.Add(fakeDir);
        }

        return fakeDirsList;
    }
    public List<List<string>> DivideUpArray(List<string> listOfDirectories, int numOfThreads)
    {
        int totalDirsCount = listOfDirectories.Count;
        var dirsPerThread = totalDirsCount/numOfThreads;
        var startingIndex = 0;
        var whatIsLeft = 0;
        var listOfDirLists = new List<List<string>>();

        while(startingIndex < totalDirsCount && listOfDirLists.Count < numOfThreads)
        {
            var lengthToGet = dirsPerThread;
            whatIsLeft = (totalDirsCount - startingIndex);

            //9000(totalDirsCount) - 8000(staringIndex) < 2000(dirsPerThread)
            if( whatIsLeft <= dirsPerThread)
            {
                lengthToGet = totalDirsCount - startingIndex;
            }

            var rangeToAdd = listOfDirectories.GetRange(startingIndex,lengthToGet);

            listOfDirLists.Add(rangeToAdd);

            startingIndex +=dirsPerThread;
        }
        
        //if there's any left add those to the last list fo dirs
        var rangeThatIsLeft = listOfDirectories.GetRange(startingIndex,(totalDirsCount - startingIndex));
        listOfDirLists[listOfDirLists.Count-1].AddRange(rangeThatIsLeft);

        return listOfDirLists;
    }
}