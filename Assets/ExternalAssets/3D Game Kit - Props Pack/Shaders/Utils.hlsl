#ifndef UTILS_INCLUDED
#define UTILS_INCLUDED

void blend_rnm_half(half3 n1, half3 n2, out half3 output)
{
    n1.z += 1;
    n2.xy = -n2.xy;

    output = n1 * dot(n1, n2) / n1.z - n2;
}

#endif