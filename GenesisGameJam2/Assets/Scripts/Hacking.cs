using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class Hacking : MonoBehaviour
{

	[SerializeField] private int Press_Count = 4;
	[SerializeField]  private int[] sequence = { 1, 9, 2, 6, 3, 3 };
	private List<int> input;


	public UnityEvent Completed, Failed;


	// Start is called before the first frame update

	public void add(int n) {
		input.Add(n);

		if (check() && input.Count == Press_Count) {
			Completed.Invoke();
			input.Clear();
		}

		if (!check()) {
			Failed.Invoke();
			input.Clear();
		}
	}

	bool check() {
		int i = 0,  j = 0;
		bool flag;

		while(j < sequence.Length && i < input.Count) {
			if (input[i] == sequence[j]) {
				i++;
			}
			j++;
		}

		if (i == input.Count)
			return true;
		else 
			return false;

	}

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
