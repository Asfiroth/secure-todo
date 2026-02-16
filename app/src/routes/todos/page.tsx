import { Suspense } from 'react';
import { View } from '~/routes/todos/Components/View';

const TodoList = () => {
  return (
    <Suspense>
      <View />
    </Suspense>
  );
};

export default TodoList;
