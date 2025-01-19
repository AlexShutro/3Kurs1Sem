class Program 
{ 
    static void Main(string[] args) 
    { 
        int mem = 0; // грубая оценка, нужно спросить у системы 
        List<Big> lbig = new List<Big>(1000); 
        while (true) 
        { 
            lbig.Add(new Big()); 
            mem += 1048576 * 128; 
            Console.WriteLine("{0,-6} MB", (mem / 1048576)); 
            Thread.Sleep(5000); 
        } 
    } 
} 
 
class Big 
{ 
    public Int32[] IntArray; 
    public Big() 
    { 
        IntArray = new int[1048576 * 32]; 
    } 
}