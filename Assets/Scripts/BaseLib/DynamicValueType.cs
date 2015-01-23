using System;
using UnityEngine;

namespace BaseLib
{
	public abstract class AbstractDynamicValue<T>
	{
		public abstract T Value { get; }

		public static implicit operator T(AbstractDynamicValue<T> adv)
		{
			if(adv == null)
			{
				return default(T);
			}
			return adv.Value;		
		}
	}

	public class FixedFloat : AbstractDynamicValue<float>
	{
		private float value;

		public override float Value
		{
			get { return value; }
		}

		public FixedFloat (float value)
		{
			this.value = value;
		}
	}

	public class RandomRangeFloat : AbstractDynamicValue<float>
	{
		private float min;
		private float max;

		public override float Value
		{
			get { return UnityEngine.Random.Range (min, max); }
		}

		public RandomRangeFloat(float rangeMin, float rangeMax)
		{
			min = rangeMin;
			max = rangeMax;
		}
	}
}