#version 110
/*

    Copyright (c) 2015 Oliver Lau <ola@ct.de>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

uniform sampler2D uTexture;
uniform float uT;
uniform float uMaxT;
uniform float uDistort;
uniform vec2 uCenter;

vec2 barrelDistortion(vec2 coord, float amt) {
  vec2 cc = coord - uCenter;
  float dist = dot(cc, cc);
  return coord + cc * dist * amt;
}

float sat(float t)
{
  return clamp(t, 0.0, 1.0);
}

float linterp(float t) {
  return sat(1.0 - abs(2.0 * t - 1.0));
}

float remap(float t, float a, float b) {
  return sat((t - a) / (b - a));
}

vec3 spectrum_offset(float t) {
  float lo = step(t, 0.5);
  float hi = 1.0 - lo;
  float w = linterp(remap(t, 1.0 / 6.0, 5.0 / 6.0));
  vec3 ret = vec3(lo, 1.0, hi) * vec3(1.0 - w, w, 1.0 - w);
  return pow(ret, vec3(1.0 / 2.2));
}

vec4 lensDistort(vec2 uv, float distort)
{
  int numIter = 10;
  float invIter = 1.0 / float(numIter);
  vec3 sumcol = vec3(0.0);
  vec3 sumw = vec3(0.0);
  float t;
  vec3 w;

  // unrolled loop (numIter iterations)

  t = 0.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 1.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 2.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 3.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 4.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 5.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 6.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 7.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 8.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  t = 9.0 * invIter;
  w = spectrum_offset(t);
  sumw += w;
  sumcol += w * texture2D(uTexture, barrelDistortion(uv, distort*t)).rgb;

  return vec4(sumcol.rgb / sumw, 1.0);
}


float quadEaseInOut(float t, float b, float c, float d)
{
  t /= d/2.0;
  if (t < 1.0) return c/2.0*t*t + b;
  t--;
  return -c/2.0 * (t*(t-2.0) - 1.0) + b;
}


void main()
{
  vec2 pos = gl_TexCoord[0].xy;
  float intensity = 1.0 - quadEaseInOut(uT, 0.0, 1.0, uMaxT);
  gl_FragColor = lensDistort(pos, uDistort * intensity);
}
