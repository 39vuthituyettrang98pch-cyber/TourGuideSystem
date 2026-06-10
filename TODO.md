# TODO - TourGuideSystem (UI/UX Glass + Safe-Area + Offline-First)

## Plan
1. [ ] Extend CSS design tokens + add missing Glass/Neumorphism utilities + map-popup animations.
   - File: `AdminWeb/src/styles/glass-safe-area.css` and/or `AdminWeb/src/styles/index.css`
2. [ ] Implement React UI components:
   - `POICard.jsx` (card quán ăn, neumorphism shadows + tap animation)
   - `POIPopup.jsx` (popup slide-up/backdrop)
   - `OfflineBanner.jsx` (listen `navigator.onLine`, show offline-first fallback)
   - Files: `AdminWeb/src/components/ui/`
3. [ ] Integrate components into existing page(s) that render POIs/map.
   - Find candidates in `AdminWeb/src/pages/*`.
4. [ ] Ensure all interactive containers use safe-area classes.
5. [ ] Offline-first UI states:
   - loading skeleton / cached content message.
6. [ ] Quick local build/run + verify responsive + notch padding.

