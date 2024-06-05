import {
  trigger,
  animate,
  transition,
  style,
  query,
  state,
  stagger
} from '@angular/animations';

export const fadeAnimation =

  trigger('fadeAnimation', [

    transition('* => *', [

      query(':enter',
        [
          style({ opacity: 0 })
        ],
        { optional: true }
      ),

      query(':leave',
        [
          style({ opacity: 1 }),
          animate('0.5s', style({ opacity: 0 }))
        ],
        { optional: true }
      ),

      query(':enter',
        [
          style({ opacity: 0 }),
          animate('0.5s', style({ opacity: 1 }))
        ],
        { optional: true }
      )

    ])

  ]);

export const listAnimation =
  trigger("listAnimation", [
    transition("* => *", [
      // each time the binding value changes
      query(
        ":leave",
        [stagger(100, [animate("0.5s", style({ opacity: 0 }))])],
        { optional: true }
      ),
      query(
        ":enter",
        [
          style({ opacity: 0 }),
          stagger(100, [animate("0.5s", style({ opacity: 1 }))])
        ],
        { optional: true }
      )
    ])
  ]);
export const enterAnimation = trigger(
  'enterAnimation', [
  transition(':enter', [
    style({ transform: 'translateX(100%)', opacity: 0 }),
    animate('500ms', style({ transform: 'translateX(0)', opacity: 1, 'overflow-x': 'hidden' }))
  ]),
  transition(':leave', [
    style({ transform: 'translateX(0)', opacity: 1 }),
    animate('500ms', style({ transform: 'translateX(100%)', opacity: 0 }))
  ])
]
);
export const slideIn =    trigger('slideIn', [
      state('*', style({ 'overflow-y': 'hidden' })),
      state('void', style({ 'overflow-y': 'hidden' })),
      transition('* => void', [
        style({ height: '*' }),
        animate(250, style({ height: 0 }))
      ]),
      transition('void => *', [
        style({ height: '0' }),
        animate(250, style({ height: '*' }))
      ])
    ]);
  


