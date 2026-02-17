import { isUndefined, isNull, isEmpty } from 'lodash';

export const queryParamAssembler = (
  queryParams: Record<string, string | number | boolean | undefined | null>,
): string => {
  const queryString = Object.entries(queryParams)
    .map(([key, value]) => {
      if (isUndefined(value) || isNull(value) || value === '') {
        return '';
      }
      return `${key}=${value}`;
    })
    .filter((query) => !isEmpty(query))
    .join('&');

  return queryString ? `?${queryString}` : '';
};
