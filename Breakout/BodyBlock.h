// Copyright (c) 2015 Oliver Lau <oliver@ersatzworld.net>
// All rights reserved.

#ifndef __BODYBLOCK_H_
#define __BODYBLOCK_H_

#include "Body.h"
#include "Breakout.h"
#include "Destructive.h"

namespace Breakout {

  class Block : public Body
  {
  public:
    Block(int index, Game *game);
#ifndef NDEBUG
    ~Block() {
      std::cout << "~dtor of " << typeid(this).name() << std::endl;
    }
#endif

    // Body implementation
    virtual void onUpdate(float elapsedSeconds);
    virtual void onDraw(sf::RenderTarget &target, sf::RenderStates states) const;
    virtual BodyType type(void) const { return Body::BodyType::Block; }

    virtual bool hit(float impulse);
  };

}

#endif // __BODYBLOCK_H_

