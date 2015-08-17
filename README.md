SafeList
========

Purpose
-------

A list that handles adding and removing elements while iterating over it. Loops using the foreach keyword, the ForEach() method, or the GetEnumerator() methods will be internally tracked by this class. Adding and removing elements during these loops will not cause exceptions to be thrown, unlike with [System.Collections.List[T]](https://msdn.microsoft.com/en-us/library/6sh2ey19(v=vs.90).aspx).

When elements are added after the current loop position, the loop will proceed to them as though they were present from the start. Elements added before the current loop position will be skipped. Elements removed before the current loop position will have no effect on the loop. Elements removed after the current loop position will not be iterated over.

Usage
-----

`SafeList` is meant to be a drop-in replacement for `System.Collections.List[T]`, so you can use it in exactly the same way.

Project
-------

There are only two files for now.

  * `SafeList`: the class
  * `TestSafeList`: unit tests requiring [NUnit](http://www.nunit.org/)

License
-------

[MIT](http://opensource.org/licenses/MIT)

Links
-----

  * [Introduction Article](http://jacksondunstan.com/articles/3179)