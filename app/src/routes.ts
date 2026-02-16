import { type RouteConfig, index, layout, route } from '@react-router/dev/routes';

export default [
  layout('./layouts/GuestLayout/GuestLayout.tsx', [
    route('login', './routes/login/page.tsx'),
  ]),
  layout('./layouts/AuthorizedLayout/AuthorizedLayout.tsx', [
    index('./routes/todos/page.tsx'),
  ]),
] satisfies RouteConfig;
