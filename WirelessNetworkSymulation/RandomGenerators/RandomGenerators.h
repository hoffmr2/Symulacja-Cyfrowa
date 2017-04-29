// RandomGenerators.h

#pragma once

class UniformGenerator;
class ExpGenerator;
using namespace System;

namespace RandomGenerators {

	public ref class UniformRandomGenerator
	{
  public:
    UniformRandomGenerator(int kernel);
    double Rand();
    double Rand(int min, int max);
    int GetKernel();
  private:
    UniformGenerator* uniform_generator_;
	};

  public ref class ExponentialRandomGenerator
  {
  public:
    ExponentialRandomGenerator(double lambda,int kernel);
    double Rand();
  private:
    ExpGenerator* exp_generator_;
  };


}
