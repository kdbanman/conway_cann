﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnvironmentStatePresets {

	private readonly static Dictionary<string, float[,]> environments = new Dictionary<string, float[,]>();

	public static float[,] Get(string name) {
		if (environments.Count == 0)
        {
            InitEnvironments();
        }
        return environments[name];
	}

    private static void InitEnvironments()
    {
		environments.Add("Glider Land", new float[,]{

			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			{ 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, },
			{ 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, },
			{ 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, },
			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },

			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			{ 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, },
			{ 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, },
			{ 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, },
			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },

			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			{ 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, },
			{ 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, },
			{ 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, },
			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },

			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			{ 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, },
			{ 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, },
			{ 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, },
			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },

			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			{ 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, },
			{ 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, },
			{ 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, },
			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },

			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			{ 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, },
			{ 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, },
			{ 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, },
			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },

			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			{ 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, },
			{ 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, },
			{ 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, },
			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },

			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			{ 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, 0,1,1,1,0, },
			{ 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, 0,0,0,1,0, },
			{ 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, 0,0,1,0,0, },
			{ 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, },
			
		});


		environments.Add("Gabriel's P138", new float[,]{

			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },

			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,1,1,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,1,1,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },

			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,1,1, 1,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,1,0,0, 1,0,0,0,0, 0,0,0,0,0, 0,0,0,1,1, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,1,0,0, 1,0,0,0,0, 0,0,0,0,0, 0,0,0,1,1, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,1,1,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },

			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,1,1,1, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,1,0,0, 1,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 1,0,0,0,0, 0,0,0,0 },

			{ 0,0,0, 0,0,0,0,0, 1,1,1,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,1,1, 1,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 1,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 1,0,0,1,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,1,1,1,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },

			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,1,1,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,1,1,0,0, 0,0,0,0,0, 0,0,0,0,0, 1,0,0,1,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,1,1,0,0, 0,0,0,0,0, 0,0,0,0,0, 1,0,0,1,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },

			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 1,1,1,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,1,1, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,1,1, 0,0,0,0,0, 0,0,0,0 },
			
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },
			{ 0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0,0, 0,0,0,0 },

		});
    }
}
