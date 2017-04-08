// This is the main DLL file.

#include "stdafx.h"

#include "RandomGenerators.h"
#include <assert.h>

RandomGenerators::UniformRandomGenerator::UniformRandomGenerator(int kernel)
{
  uniform_generator_ = new UniformGenerator(kernel);
}

double RandomGenerators::UniformRandomGenerator::Rand()
{
  assert(uniform_generator_ != nullptr);
  return uniform_generator_->Rand();
}

double RandomGenerators::UniformRandomGenerator::Rand(int min, int max)
{
  assert(uniform_generator_ != nullptr);
  return uniform_generator_->Rand(min,max);
}

int RandomGenerators::UniformRandomGenerator::GetKernel()
{
  assert(uniform_generator_ != nullptr);
  return uniform_generator_->get_kernel();
}

RandomGenerators::ExponentialRandomGenerator::ExponentialRandomGenerator(double lambda, int kernel)
{
  exp_generator_ = new ExpGenerator(lambda, new UniformGenerator(kernel));
}

double RandomGenerators::ExponentialRandomGenerator::Rand()
{
  assert(exp_generator_ != nullptr);
    return exp_generator_->Rand();
}
